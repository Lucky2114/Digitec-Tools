using System;
using System.Linq;
using System.Threading.Tasks;
using Shopping_Tools_Api_Services.Config;
using Shopping_Tools_Api_Services.Core.Digitec;
using Shopping_Tools_Api_Services.Models;

namespace Shopping_Tools_Api_Services.Core.Mediamarkt
{
    public class Mediamarkt : IApi
    {
        public Mediamarkt()
        {
            OnlineShopName = "Mediamarkt";
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

            return uri.Host.Equals("www.mediamarkt.ch", StringComparison.OrdinalIgnoreCase) && uri.AbsolutePath.Contains("/product/");
        }

        public async Task<Product> GetProductInfo(string productUrl)
        {
            if (!IsValidUrl(productUrl)) return null;

            var doc = await HttpHelper.GetDocument(productUrl);
            var productDetailNode = doc.DocumentNode.Descendants("div")
                .First(x => x.Id.Equals(MediamarktWebConstants.ProductDetailId));
            
            
            var productTitleNode = productDetailNode.Descendants("h1")
                .First(x => x.Attributes["itemprop"].Value.Equals("name"));
            
            var name = productTitleNode.InnerText;

            var priceDetailNode = doc.DocumentNode.Descendants("div")
                .First(x => x.HasClass(MediamarktWebConstants.PriceDetailClassName));
            var priceNode = priceDetailNode.Descendants("meta")
                .First(x => x.Attributes["itemprop"].Value.Equals("price"));
            var price = priceNode.Attributes["content"].Value;

            /*var priceOldNode = doc.DocumentNode.Descendants()
                .First(x => x.HasClass(BrackWebConstants.ProductPricingSectionClassName)).Descendants("span")
                .First(x => x.HasClass(BrackWebConstants.ProductOldPriceClassName))
                .Descendants().First(x => x.Attributes.Any(y => y.Name.Equals("content")));
            var priceOld = priceOldNode.InnerText.Trim();*/

            return await Task.FromResult(new Product()
            {
                Name = name,
                Brand = "",
                Url = productUrl,
                OnlineShopName = OnlineShopName,
                PriceCurrent = price,
                PriceOld = "",
                ProductId = productUrl,
                ProductIdSimple = Helpers.ExtractIdFromUrl(productUrl)
            });
        }

        public string OnlineShopName { get; }
    }
}