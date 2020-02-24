using Newtonsoft.Json;
using Shopping_Tools_Api_Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Shopping_Tools_Api_Services.Core
{
    public class ProxyHelper
    {
        private const string ProxyCacheFilePath = "proxies.json";

        private static ProxyHelper _instance;
        private bool _requestLimitReached;

        private readonly ReaderWriterLock _locker = new ReaderWriterLock();

        public static ProxyHelper GetInstance()
        {
            return _instance ?? (_instance = new ProxyHelper());
        }

        public void TryAddProxyToCache(WebProxy proxy)
        {
            List<WebProxy> proxies = GetCachedProxies();

            //Only add the cache if it doesn't already exist
            if (!proxies.Any(x => x.Address.AbsoluteUri.Equals(proxy.Address.AbsoluteUri)))
            {
                proxies.Add(proxy);
                Console.WriteLine($"Added Proxy {proxy.Address} to cache");
            }

            var jsonData = JsonConvert.SerializeObject(proxies);
            try
            {
                _locker.AcquireWriterLock(TimeSpan.FromSeconds(10));
                File.WriteAllText(ProxyCacheFilePath, jsonData);
            }
            finally
            {
                _locker.ReleaseWriterLock();
            }
        }

        private List<WebProxy> GetCachedProxies()
        {
            List<WebProxy> proxies = new List<WebProxy>();

            if (File.Exists(ProxyCacheFilePath))
            {
                using (var stream = File.Open(ProxyCacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var jsonString = new StreamReader(stream).ReadToEnd();
                    //If the deserializing returns null, set it to a new, empty list.
                    proxies = JsonConvert.DeserializeObject<List<WebProxy>>(jsonString) ?? new List<WebProxy>();
                }
            }
            return proxies;
        }

        private void ClearCachedProxies()
        {
            try
            {
                _locker.AcquireWriterLock(TimeSpan.FromSeconds(10));
                File.Delete(ProxyCacheFilePath);
            }
            finally
            {
                _locker.ReleaseWriterLock();
            }
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
                var cachedProxies = GetCachedProxies();
                Console.WriteLine($"Reached request limit. {cachedProxies.Count} proxies in cache.");
                //We're probably out of requests.
                _requestLimitReached = true;
                //Return a "cached" proxy.
                if (cachedProxies.Count > 0)
                {
                    var chachedProxy = cachedProxies[new Random().Next(cachedProxies.Count - 1)];
                    Console.WriteLine($"Using cached proxy: {chachedProxy?.Address}");
                    return chachedProxy;
                }
            }
            RestProxyResult proxyResult = JsonConvert.DeserializeObject<RestProxyResult>(jsonResult);

            if (proxyResult != null)
            {
                if (_requestLimitReached)
                {
                    //This probably means that the request limit is once again refilled.
                    _requestLimitReached = false;
                    //Clear the cached proxies. They could be outdated by now.
                    ClearCachedProxies();
                }

                proxy = new WebProxy(proxyResult.ip, proxyResult.port);
            }

            if (proxy != null)
                Console.WriteLine($"Using proxy: {proxy.Address}");
            else Console.WriteLine("Not using any proxy");
            return proxy;
        }
    }
}