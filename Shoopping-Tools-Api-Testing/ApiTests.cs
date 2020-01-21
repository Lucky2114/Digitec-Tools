using Shopping_Tools_Api_Services;
using NUnit.Framework;
using System.Threading.Tasks;
using Shopping_Tools_Api_Services.Core.Digitec;

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
            var res = await new Digitec().GetProductInfo(productUrl);
            Assert.IsNotNull(res);
        }
    }
}