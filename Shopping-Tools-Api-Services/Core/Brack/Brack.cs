using System;
using System.Linq;
using System.Threading.Tasks;
using Shopping_Tools_Api_Services.Config;
using Shopping_Tools_Api_Services.Core.Digitec;
using Shopping_Tools_Api_Services.Models;

namespace Shopping_Tools_Api_Services.Core.Brack
{
    public class Brack : IApi
    {
        public string OnlineShopName { get; }

        //The TestUrl is needed for the unit tests
        public string TestUrl { get => "https://www.brack.ch/sklz-basketball-micro-ball-673632"; set { } }

        public Brack()
        {
            OnlineShopName = "Brack";
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

            uri = uri.RemoveParameters();
            return uri.Host.Equals("www.brack.ch", StringComparison.OrdinalIgnoreCase) &&
                   char.IsDigit(uri.AbsolutePath.ToCharArray().Last());
        }

        public async Task<Product> GetProductInfo(string productUrl)
        {
            if (!IsValidUrl(productUrl)) return null;

            var doc = await HttpHelper.GetDocument(productUrl);
            if (doc == null)
                throw new Exception("Brack is not responding!");


            var productTitleNode = doc.DocumentNode.Descendants()
                .First(x => x.HasClass(BrackWebConstants.ProductDetailClassName)).Descendants("h1").First();
            var name = productTitleNode.InnerText;

            var brandNode = doc.DocumentNode.Descendants()
                .First(x => x.HasClass(BrackWebConstants.ProductDetailClassName)).Descendants("span")
                .First(x => x.HasClass(BrackWebConstants.ProductManufacturerClassName));
            var brand = brandNode.InnerText;

            var priceNode = doc.DocumentNode.Descendants()
                .First(x => x.HasClass(BrackWebConstants.ProductPricingSectionClassName)).Descendants("span")
                .First(x => x.HasClass(BrackWebConstants.ProductPriceClassName))
                .Descendants().First(x => x.Attributes.Any(y => y.Name.Equals("content")));
            var price = priceNode.InnerText.Trim();

            var priceOld = "";
            try
            {
                var priceOldNode = doc.DocumentNode.Descendants()
                    .First(x => x.HasClass(BrackWebConstants.ProductPricingSectionClassName)).Descendants("span")
                    .First(x => x.HasClass(BrackWebConstants.ProductOldPriceClassName))
                    .Descendants().First(x => x.Attributes.Any(y => y.Name.Equals("content")));
                priceOld = priceOldNode.InnerText.Trim();
            }
            catch
            {
                //No old price
            }

            return await Task.FromResult(new Product()
            {
                Name = name,
                Brand = brand,
                Url = productUrl,
                OnlineShopName = OnlineShopName,
                PriceCurrent = price,
                PriceOld = priceOld,
                ProductId = productUrl,
                ProductIdSimple = Helpers.ExtractIdFromUrl(productUrl),
                Currency = "CHF" //It's Brack..
            });
        }
    }
}