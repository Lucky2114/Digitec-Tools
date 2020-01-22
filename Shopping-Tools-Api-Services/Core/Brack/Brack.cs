using System.Threading.Tasks;
using Shopping_Tools_Api_Services.Core.Digitec;
using Shopping_Tools_Api_Services.Models;

namespace Shopping_Tools_Api_Services.Core.Brack
{
    public class Brack : IApi
    {
        public Brack()
        {
            OnlineShopName = "Brack";
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
                ProductIdSimple = "TESTID_SIMPLE"
            });
        }

        public string OnlineShopName { get; }
    }
}