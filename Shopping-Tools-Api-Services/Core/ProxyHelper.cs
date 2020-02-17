using Newtonsoft.Json;
using Shopping_Tools_Api_Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Shopping_Tools_Api_Services.Core
{
    public class ProxyHelper
    {
        private static ProxyHelper _instance;

        private readonly List<WebProxy> _usedProxies;
        private bool _requestLimitReached;

        public static ProxyHelper GetInstance()
        {
            return _instance ?? (_instance = new ProxyHelper());
        }

        private ProxyHelper()
        {
            _usedProxies = new List<WebProxy>();
        }

        public WebProxy GetRandomProxy()
        {
            WebProxy proxy = null;
            string jsonResult = "";
            try
            {
                jsonResult = new WebClient().DownloadString("https://api.getproxylist.com/proxy?protocol[]=http&allowsHttps=1&maxConnectTime=2");
                //jsonResult = new WebClient().DownloadString("http://pubproxy.com/api/proxy?type=http&https=true");
            }
            catch
            {
                Console.WriteLine($"Reached request limit. {_usedProxies.Count} proxies in cache.");
                //We're probably out of requests.
                _requestLimitReached = true;
                //Return a "cached" proxy.
                if (_usedProxies.Count > 0)
                {
                    Console.WriteLine($"Using cached proxy: {proxy.Address}");
                    return _usedProxies[new Random().Next(_usedProxies.Count - 1)];
                }
            }
            RestProxyResult proxyResult = JsonConvert.DeserializeObject<RestProxyResult>(jsonResult);

            if (proxyResult != null)
            {
                if (_requestLimitReached)
                {
                    //This probably means that the request limit is once again refilled.
                    _requestLimitReached = false;
                    //Flush the proxies list. They could be outdated by now.
                    _usedProxies.Clear();
                }

                proxy = new WebProxy(proxyResult.ip, proxyResult.port);
                if (!_usedProxies.Any(x => x.Address.AbsoluteUri.Equals(proxy.Address.AbsoluteUri)))
                {
                    _usedProxies.Add(proxy);
                }
            }

            if (proxy != null)
                Console.WriteLine($"Using proxy: {proxy.Address}");
            else Console.WriteLine("Not using any proxy");
            return proxy;
        }
    }
}