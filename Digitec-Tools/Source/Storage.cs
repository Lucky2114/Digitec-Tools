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

        public async Task AddNewProduct(Product product, UserData userData)
        {
            if (product.ProductIdSimple.Equals(""))
                throw new Exception("Product contains no Id");

            var collection = database.Collection("Products");
            var document = collection.Document(product.ProductIdSimple);

            //TODO document need to contain:
            // -values of the product (name, brand etc)
            // -list of users

            //if the product already exists, add the user to the collection. (check for existing there first)
            var existing = await document.GetSnapshotAsync();
            if (!existing.Exists)
            {
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Name", product.Name },
                    { "Brand", product.Brand },
                    { "UserEmail", userData.Email },
                    { "UserIPv4", userData.IPv4 }
                };

                await document.SetAsync(data);
            }
        }
    }
}