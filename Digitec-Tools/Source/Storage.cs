using Digitec_Api.Models;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digitec_Tools.Source
{
    public class Storage
    {
        private FirestoreDb database;

        public Storage()
        {
            database = FirestoreDb.Create("digitec-tools");
        }

        public async Task<bool> AddNewProduct(Product product, UserData userData)
        {
            try
            {
                if (product.ProductIdSimple.Equals(""))
                    throw new Exception("Product contains no Id");

                var collection = database.Collection("Products");
                var document = collection.Document(product.ProductIdSimple);

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Name", product.Name },
                    { "Brand", product.Brand },
                    { "PriceCurrent", product.PriceCurrent },
                    { "PriceOld", product.PriceOld },
                    { "ProductIdSimple", product.ProductIdSimple }
                };

                await document.SetAsync(data);



                var userCollection = document.Collection("Users");
                var userDocument = userCollection.Document(userData.Email);

                Dictionary<string, object> userDocData = new Dictionary<string, object>
                {
                    { "Email", userData.Email},
                    { "IPv4", userData.IPv4}
                };

                await userDocument.SetAsync(userDocData);
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }
    }
}