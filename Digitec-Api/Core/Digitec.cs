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
                var _productTitleNode = doc.DocumentNode.Descendants().Where(x => x.GetClasses().SequenceEqual(DigitecWebConstatnts.ProductNameClasses)).First();

                string _brand = _productTitleNode.Descendants("strong").TryFirst().InnerText;
                string _productName = _productTitleNode.Descendants("span").TryFirst().InnerText;

                var _productPriceNode = doc.DocumentNode.Descendants().Where(x => x.GetClasses().SequenceEqual(DigitecWebConstatnts.ProductPriceClasses)).First();

                string _priceCurrent = _productPriceNode.Descendants("strong").TryFirst().InnerText;
                string _priceOld = _productPriceNode.Descendants("span").TryFirst().InnerText;

                Product productInfo = new Product()
                {
                    Brand = _brand.Trim(),
                    Name = _productName.Trim(),
                    PriceCurrent = _priceCurrent.Trim(),
                    PriceOld = _priceOld.Trim()
                };
                return await Task.FromResult(productInfo);
            }
            else
            {
                return null;
            }
        }
    }
}