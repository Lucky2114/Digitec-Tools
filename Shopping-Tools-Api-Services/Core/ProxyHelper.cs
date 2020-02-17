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
            var jsonResult = new WebClient().DownloadString("https://api.getproxylist.com/proxy?protocol[]=http&allowsHttps=1");
            RestProxyResult proxyResult = null;

            try
            {
                proxyResult = JsonConvert.DeserializeObject<RestProxyResult>(jsonResult);
            }
            catch
            {
                //We're probably out of requests.
                _requestLimitReached = true;
                //Return a "cached" proxy.
                if (_usedProxies.Count > 0)
                    return _usedProxies[new Random().Next(_usedProxies.Count - 1)];
            }

            if (proxyResult != null)
            {
                if (_requestLimitReached)
                {
                    //This probably means that the request limit is once again refilled.
                    _requestLimitReached = false;
                    //Flush the proxies list. They could be outdated by now.
                    _usedProxies.Clear();
                }

                proxy = new WebProxy($"{proxyResult.protocol}://{proxyResult.ip}", proxyResult.port);
                if (!_usedProxies.Any(x => x.Address.AbsoluteUri.Equals(proxy.Address.AbsoluteUri)))
                {
                    _usedProxies.Add(proxy);
                }

            }

            return proxy;
        }
    }
}