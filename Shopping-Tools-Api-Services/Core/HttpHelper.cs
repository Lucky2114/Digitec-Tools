using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        internal static async Task<HtmlDocument> GetDocument(string url)
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                          "ToasterOS; .NET CLR 1.0.3705;)");
            var html = await client.DownloadStringTaskAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return await Task.FromResult(doc);
        }
    }
}