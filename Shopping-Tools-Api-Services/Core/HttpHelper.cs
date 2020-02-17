using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        //TODO Create list of user agents to rotate
        private const string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; de-DE; rv:x.x.x) Gecko/20041107 Firefox/x.x";

        internal static async Task<HtmlDocument> GetDocument(string url, bool fastRequest)
        {
            var client = new WebClient();
            if (!fastRequest)
                client.Proxy = ProxyHelper.GetInstance().GetRandomProxy();

            client.Headers.Add("User-Agent", _userAgent);
            client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

            try
            {
                var source = await client.DownloadStringTaskAsync(url);

                Console.WriteLine($"Request through proxy successful");

                var doc = new HtmlDocument();
                doc.LoadHtml(source);
                return await Task.FromResult(doc);
            }
            catch
            {
                Console.WriteLine("Connection through proxy failed. Trying again.");
                return await GetDocument(url, fastRequest);
            }
        }
    }
}