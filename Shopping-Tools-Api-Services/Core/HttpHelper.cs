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

        internal static async Task<HtmlDocument> GetDocument(string url, bool fastRequest, int failedAttemps = 0)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var proxy = ProxyHelper.GetInstance().GetRandomProxy();
            if (!fastRequest)
                request.Proxy = proxy;

            //request.Headers.Add("User-Agent", _userAgent);
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            //request.Encoding = Encoding.UTF8;
            request.Method = "GET";
            request.Timeout = 10000;
            request.Headers = GetHeaderCollection(url);

            try
            {
                if (failedAttemps > 5)
                {
                    Console.WriteLine("Too many failed attemps. Using no proxy.");
                    request.Proxy = null;
                }

                var source = await request.GetResponseAsync();
                if (proxy != null)
                {
                    Console.WriteLine($"Request through proxy successful. URL = {url}");
                    ProxyHelper.GetInstance().TryAddProxyToCache(proxy);
                }
                else
                {
                    Console.WriteLine("Request without proxy successful");
                }
                var doc = new HtmlDocument();
                doc.Load(source.GetResponseStream());
                return await Task.FromResult(doc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection through proxy failed. Trying again. \n" +
                    $"Exception: {ex.Message}");
                int tmp = failedAttemps += 1;
                return await GetDocument(url, fastRequest, tmp);
            }
        }

        private static WebHeaderCollection GetHeaderCollection(string url)
        {
            var uri = new Uri(url);
            return new WebHeaderCollection
            {
                { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" },
                {  "Accept-Encoding", "gzip, deflate, br"},
                { "Accept-Language", "de-CH,de-DE;q=0.9,de;q=0.8,en-US;q=0.7,en;q=0.6"},
                { "Cache-Control", "max-age=0"},
                { "Host", $"{uri.Host}" },
                { "Referer", "https://www.google.ch"},
                { "Sec-Fetch-Mode", "navigate"},
                {"Sec-Fetch-Site", "cross-site" },
                { "Sec-Fetch-User", "?1"},
                { "Upgrade-Insecure-Requests", "1" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36"},
            };
        }
    }
}