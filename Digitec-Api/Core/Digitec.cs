using Digitec_Api.Config;
using Digitec_Api.Core;
using Digitec_Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Digitec_Api
{
    public static class Digitec
    {
        public static async Task<Product> GetProductInfo(string productUrl)
        {
            if (Validation.IsValidDigitecUrl(productUrl))
            {
                var doc = await HttpHelper.GetDocument(productUrl);
                var productTitleNode = doc.DocumentNode.Descendants().First(x => x.GetClasses().SequenceEqual(DigitecWebConstatnts.ProductNameClasses));

                string brand = productTitleNode.Descendants("strong").TryFirst().InnerText;
                string productName = productTitleNode.Descendants("span").TryFirst().InnerText;

                var productPriceNode = doc.DocumentNode.Descendants().First(x => x.GetClasses().SequenceEqual(DigitecWebConstatnts.ProductPriceClasses));

                string priceCurrent = productPriceNode.Descendants("strong").TryFirst().InnerText;
                string priceOld = productPriceNode.Descendants("span").TryFirst().InnerText;

                Product productInfo = new Product()
                {
                    Brand = brand.Trim(),
                    Name = productName.Trim(),
                    PriceCurrent = priceCurrent.Trim(),
                    PriceOld = priceOld.Trim(),
                    ProductId = productUrl
                };
                return await Task.FromResult(productInfo);
            }
            return null;
        }
    }
}