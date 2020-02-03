using Shopping_Tools_Api_Services.Config;
using Shopping_Tools_Api_Services.Core.Digitec;
using Shopping_Tools_Api_Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core.Mediamarkt
{
    public class Mediamarkt : IApi
    {
        public string TestUrl { get => "https://www.mediamarkt.ch/de/product/_apple-airpods-2019-2nd-gen-1913640.html"; set { } }

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
            //var productDetailNode = doc.DocumentNode.Descendants("div")
            //    .First(x => x.Id.Equals(MediamarktWebConstants.ProductDetailId));

            //var productTitleNode = productDetailNode.Descendants("h1")
            //    .First(x => x.Attributes["itemprop"].Value.Equals("name"));

            //var name = productTitleNode.InnerText;

            //var priceDetailNode = doc.DocumentNode.Descendants("div")
            //    .First(x => x.HasClass(MediamarktWebConstants.PriceDetailClassName));

            var metaTags = doc.DocumentNode.SelectNodes("//meta");

            var brand = "";
            var currency = "";
            var price = "";
            var name = "";

            foreach (var item in metaTags)
            {
                var att = item.Attributes["property"];
                if (att == null) continue;
                switch (att.Value)
                {
                    case "product:brand":
                        brand = item.Attributes["content"].Value;
                        break;

                    case "product:price:currency":
                        currency = item.Attributes["content"].Value;
                        break;

                    case "product:price:amount":
                        price = item.Attributes["content"].Value;
                        break;
                    case "og:title":
                        name = item.Attributes["content"].Value;
                        break;
                }
            }

            return await Task.FromResult(new Product()
            {
                Name = name,
                Brand = brand,
                Url = productUrl,
                OnlineShopName = OnlineShopName,
                PriceCurrent = price,
                PriceOld = "",
                ProductId = productUrl,
                ProductIdSimple = Helpers.ExtractIdFromUrl(productUrl),
                Currency = currency
            });
        }

        public string OnlineShopName { get; }
    }
}