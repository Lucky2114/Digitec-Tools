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

            var priceOldNode = doc.DocumentNode.Descendants()
                .First(x => x.HasClass(BrackWebConstants.ProductPricingSectionClassName)).Descendants("span")
                .First(x => x.HasClass(BrackWebConstants.ProductOldPriceClassName))
                .Descendants().First(x => x.Attributes.Any(y => y.Name.Equals("content")));
            var priceOld = priceOldNode.InnerText.Trim();

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
                Currency = "CHF" //I think it's safe to assume, that brack only has CHF as currency
            });
        }
    }
}