using System;
using System.Linq;
using System.Threading.Tasks;
using Shopping_Tools_Api_Services.Config;
using Shopping_Tools_Api_Services.Models;

namespace Shopping_Tools_Api_Services.Core.Digitec
{
    public class Digitec : IApi
    {
        public string OnlineShopName { get; }

        //The TestUrl is needed for the unit tests
        public string TestUrl { get => "https://www.digitec.ch/de/s1/product/raspberry-pi-4-4g-model-b-full-starter-kit-universal-armv8-entwicklungsboard-kit-12428788"; set { } }

        public Digitec()
        {
            OnlineShopName = "Digitec";
        }

        public bool IsValidUrl(string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch
            {
                //Url is not in the correct format
                return false;
            }

            return uri.Host.Equals("www.digitec.ch", StringComparison.OrdinalIgnoreCase) && uri.AbsolutePath.Contains("/product/");
        }

        public async Task<Product> GetProductInfo(string productUrl)
        {
            if (!IsValidUrl(productUrl)) return null;
            //Problem: CSS classes are randomized => use index

            var doc = await HttpHelper.GetDocument(productUrl);
            var productTitleNode = doc.DocumentNode.Descendants().First(x => x.HasClass(DigitecWebConstants.ProductDetailClassName)).Descendants("h1").ToList()[DigitecWebConstants.ProductNameH1Index];

            var brand = productTitleNode.Descendants("strong").TryFirst().InnerText;
            var productName = productTitleNode.Descendants("span").TryFirst().InnerText;

            //There could be a progress bar (when the product is on sale)
            //remove it.
            try
            {
                var pbar = doc.DocumentNode.Descendants().First(x => x.HasClass(DigitecWebConstants.ProductDetailClassName)).Descendants("strong").First(x => x.InnerText.StartsWith("noch"));
                pbar?.ParentNode.Remove();
            }
            catch
            {
                //ignored (there is no progress bar continue)
            }

            //There could also be a label with "-15%"
            //remove it
            try
            {
                var label = doc.DocumentNode.Descendants().First(x => x.HasClass(DigitecWebConstants.ProductDetailClassName)).Descendants("div").First(x => x.InnerText.EndsWith("%"));
                label?.ParentNode.Remove();
            } catch
            {
                //ignored (there is no label)
            }


            var productPriceNode = doc.DocumentNode.Descendants().First(x => x.HasClass(DigitecWebConstants.ProductDetailClassName)).Descendants("div").ToList()[DigitecWebConstants.ProductPriceDivIndex];

            var priceCurrent = productPriceNode.Descendants("strong").TryFirst().InnerText;
            var priceOld = productPriceNode.Descendants("span").Where(x => x.HasClass(DigitecWebConstants.ProductPriceOldClassName)).TryFirst().InnerText;

            var productInfo = new Product()
            {
                Brand = brand.Trim(),
                Name = productName.Trim(),
                PriceCurrent = priceCurrent.Trim(),
                PriceOld = priceOld.Trim(),
                ProductId = productUrl,
                ProductIdSimple = Helpers.ExtractIdFromUrl(productUrl),
                Url = productUrl,
                OnlineShopName = "Digitec",
                Currency = "CHF" //I think it's safe to assume, that digitec only has CHF as currency
            };
            return await Task.FromResult(productInfo);
        }

    }
}