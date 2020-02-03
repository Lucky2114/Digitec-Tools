using NUnit.Framework;
using Shopping_Tools.Source;
using Shopping_Tools_Api_Services.Models;
using System.Threading.Tasks;

namespace Digitec_Api_Testing
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestAllApis()
        {
            foreach (var apiInstance in DynamicApiHelper.GetAllImplementingClasses())
            {
                var res = await apiInstance.GetProductInfo(apiInstance.TestUrl);
                Assert.IsTrue(CheckProductInfo(res));
            }
        }

        private bool CheckProductInfo(Product product)
        {
            bool res = true;
            if (string.IsNullOrEmpty(product.Name) || string.IsNullOrEmpty(product.Url))
                res = false;
            try
            {
                if (product.PriceCurrent.ParseToDouble() <= 0)
                    res = true;
            }
            catch
            {
                res = false;
            }

            return res;
        }
    }
}