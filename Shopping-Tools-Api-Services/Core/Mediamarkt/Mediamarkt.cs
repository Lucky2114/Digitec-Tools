﻿using System.Threading.Tasks;
using Shopping_Tools_Api_Services.Core.Digitec;
using Shopping_Tools_Api_Services.Models;

namespace Shopping_Tools_Api_Services.Core.Mediamarkt
{
    public class Mediamarkt : IApi
    {
        public Mediamarkt()
        {
            OnlineShopName = "Mediamarkt";
        }
        public async Task<Product> GetProductInfo(string productUrl)
        {
            return await Task.FromResult(new Product()
            {
                Name = "Testproduct",
                Brand = "Testbrand",
                Url = "Testurl",
                OnlineShopName = OnlineShopName,
                PriceCurrent = "100",
                PriceOld =  "200",
                ProductId = "TestID",
                ProductIdSimple = "TESTID_SIMPLE_MM"
            });
        }

        public string OnlineShopName { get; }
    }
}