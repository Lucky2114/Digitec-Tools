using Digitec_Api.Models;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Digitec_Tools_Web.Source
{
    public class Storage
    {
        private readonly FirestoreDb _database;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private static Storage _instance;

        public static Storage GetInstance(AuthenticationStateProvider authenticationStateProvider)
        {
            return _instance ??= new Storage(authenticationStateProvider);
        }

        public Storage(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _database = FirestoreDb.Create("digitec-tools");
        }

        public async Task<bool> AddNewProduct(Product product, UserData userData)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (!authState.User.Identity.IsAuthenticated)
                return await Task.FromResult(false);

            try
            {
                if (product.ProductIdSimple.Equals(""))
                    throw new Exception("Product contains no Id");

                var collection = _database.Collection("Products");
                var document = collection.Document(product.ProductIdSimple);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    {"Name", product.Name},
                    {"Brand", product.Brand},
                    {"PriceCurrent", product.PriceCurrent},
                    {"PriceOld", product.PriceOld},
                    {"ProductIdSimple", product.ProductIdSimple},
                    {"Url", product.Url}
                };

                await document.SetAsync(data);

                var userCollection = document.Collection("Users");
                var userDocument = userCollection.Document(userData.Email);

                Dictionary<string, object> userDocData = new Dictionary<string, object>
                {
                    {"Email", userData.Email},
                    {"IPv4", userData.IPv4}
                };

                var result = await userDocument.SetAsync(userDocData);
                return result != null;
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<List<Product>> GetProductsForUser()
        {
            //TODO Is there some way to do this cleaner? With queries?


            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            if (!user.Identity.IsAuthenticated)
                return null;

            var res = new List<Product>();
            //Select from firestore where user collection contains user.Name

            var query = _database.CollectionGroup("Users").WhereEqualTo("Email", user.Identity.Name);
            var querySnapshot = await query.GetSnapshotAsync();
            var matchedUsers = querySnapshot.Documents.ToList();
            
            foreach (var matchedUser in matchedUsers)
            {
                var product = await matchedUser.Reference.Parent.Parent.GetSnapshotAsync();
                product.TryGetValue("ProductIdSimple", out string id);
                product.TryGetValue("Brand", out string brand);
                product.TryGetValue("Name", out string name);
                product.TryGetValue("Url", out string url);
                res.Add(new Product()
                {
                    ProductIdSimple = id,
                    Brand = brand,
                    Url = url,
                    Name = name
                });
            }

            return await Task.FromResult(res);
        }

        public async Task<bool> RemoveUserFromProduct(Product product)
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;

            var productCollectionReference = _database.Collection("Products");
            var productReference = productCollectionReference.Document(product.ProductIdSimple);

            var userReference = productReference?.Collection("Users").Document(user.Identity.Name);

            if (userReference != null)
            {
                var result = await userReference?.DeleteAsync();
                return result != null;
            }

            return await Task.FromResult(false);
        }
    }
}