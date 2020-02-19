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
                } else
                {
                    Console.WriteLine("Request without proxy successful");
                }
                var doc = new HtmlDocument();
                doc.Load(source.GetResponseStream());
                return await Task.FromResult(doc);
            }
            catch
            {
                Console.WriteLine("Connection through proxy failed. Trying again.");
                int tmp = failedAttemps += 1;
                return await GetDocument(url, fastRequest, tmp);
            }
        }
    }
}