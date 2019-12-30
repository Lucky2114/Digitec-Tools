using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Digitec_Api.Core
{
    internal class HttpHelper
    {
        internal static async Task<HtmlDocument> GetDocument(string url)
        {
            //url has to be tested here
            var web = new HtmlWeb();
            return await web.LoadFromWebAsync(url);
        }
    }
}
