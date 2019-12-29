using Digitec_Api;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Digitec_Api_Testing
{
    public class Tests
    {
        private string productUrl = "https://www.digitec.ch/de/s1/product/lg-oled55b9-55-4k-oled-tv-10882255";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ProductInfoTest()
        {
            var res = await Digitec.GetProductInfo(productUrl);
            Assert.IsNotNull(res);
        }
    }
}