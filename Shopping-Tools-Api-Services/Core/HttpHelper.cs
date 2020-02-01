using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Cache;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        internal static async Task<HtmlDocument> GetDocument(string url)
        {
            // TODO: 
            // This is the only way I found to bypass the server cache.
            // Problem: It's very slow. ~4 seconds per request.   
            var web = new HtmlWeb();
            return await web.LoadFromWebAsync(url);

            //var client = new WebClient
            //{
            //    CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
            //};
            //var html = await client.DownloadStringTaskAsync(url);
            //var doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //return await Task.FromResult(doc);
        }
    }
}