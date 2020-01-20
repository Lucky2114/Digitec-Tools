using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core
{
    internal class HttpHelper
    {
        internal static async Task<HtmlDocument> GetDocument(string url)
        {
            var web = new WebClient();
            string html = await web.DownloadStringTaskAsync(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return await Task.FromResult(doc);
        }
    }
}
