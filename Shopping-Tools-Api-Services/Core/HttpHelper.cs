using HtmlAgilityPack;
using Newtonsoft.Json;
using Shopping_Tools_Api_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        //TODO Create list of user agents to rotate
        //TOOD Create list of proxys to use
        private static readonly List<WebProxy> _workingProxies = new List<WebProxy>();

        private const string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; de-DE; rv:x.x.x) Gecko/20041107 Firefox/x.x";

        private static bool _proxyLimitReached;

        internal static async Task<HtmlDocument> GetDocument(string url)
        {
            //Uri uri = new Uri(url);
            //var req = WebRequest.Create(uri) as HttpWebRequest;

            //req.Proxy = GetRandomProxy();
            //req.UserAgent = _userAgent;
            //req.Method = "GET";
            //req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            //var source = await req.GetResponseAsync();
            //var stream = source.GetResponseStream();

            //var doc = new HtmlDocument();
            //doc.Load(stream, true);
            //return await Task.FromResult(doc);

            var web = new HtmlWeb();
            return await web.LoadFromWebAsync(url);
        }

        internal static WebProxy GetRandomProxy()
        {
            var client = new WebClient();
            var result = client.DownloadString(new Uri("https://api.getproxylist.com/proxy?protocol[]=http&allowsHttps=1"));

            WebProxy webProxy = null;
            try
            {
                RestProxyResult proxy = JsonConvert.DeserializeObject<RestProxyResult>(result);
                if (proxy != null)
                {
                    if (_proxyLimitReached)
                    {
                        //A new day. Another 25 requests at our disposal.
                        //flush the old proxies (they could be out of date)
                        _workingProxies.Clear();
                        _proxyLimitReached = false;
                    }

                    Console.WriteLine($"Using proxy in {proxy.country}");
                    webProxy = new WebProxy()
                    {
                        Address = new Uri($"http://{proxy.ip}:{proxy.port}")
                    };
                    if (!_workingProxies.Any(x => x.Address.Equals(webProxy.Address)))
                        _workingProxies.Add(webProxy);
                }
            }
            catch
            {
                Console.WriteLine($"Proxy limit is reached! Currenty {_workingProxies.Count} proxies cached.");
                _proxyLimitReached = true;
                //Limit reached
                //Return a random proxy from the list
                if (_workingProxies.Count > 0)
                    webProxy = _workingProxies[new Random().Next(_workingProxies.Count)];
            }

            return webProxy;
        }
    }
}