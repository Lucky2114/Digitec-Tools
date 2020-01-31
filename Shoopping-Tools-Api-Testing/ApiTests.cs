using NUnit.Framework;
using Shopping_Tools.Source;
using Shopping_Tools_Api_Services.Models;
using System.Threading.Tasks;

namespace Digitec_Api_Testing
{
    public class Tests
    {
        private string productUrl = "https://www.digitec.ch/de/s1/product/lg-oled55b9-55-4k-oled-tv-10882255";

        [SetUp]
        public void Setup()
        {
            ProductInfoTest();
        }

        [Test]
        public async Task ProductInfoTest()
        {
            foreach (var apiInstance in DynamicApiHelper.GetAllImplementingClasses())
            {
                var res = await apiInstance.GetProductInfo(apiInstance.TestUrl);
                Assert.IsTrue(CheckProductInfo(res));
            }
        }

        private bool CheckProductInfo(Product product)
        {
            bool error = false;
            if (string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Url))
                error = true;
            try
            {
                if (product.PriceCurrent.ParseToDouble() <= 0)
                    error = true;
            }
            catch
            {
                error = true;
            }

            return error;
        }
    }
}