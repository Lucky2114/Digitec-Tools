using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digitec_Api.Models;
using Google.Cloud.Firestore;

namespace Digitec_Tools.Source
{
    public class Storage
    {
        private FirestoreDb database;

        public Storage()
        {
            database = FirestoreDb.Create("Products");
        }

        public async Task AddNewProduct(Product product, UserData userData)
        {
            var collection = database.Collection("Products");

            Dictionary<Product, UserData> data = new Dictionary<Product, UserData>
            {
                { product, userData }
            };

            var documentReference = await collection.AddAsync(data);
        }
    }
}
