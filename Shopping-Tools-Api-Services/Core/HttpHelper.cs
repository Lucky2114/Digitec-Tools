using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        private const string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; de-DE; rv:x.x.x) Gecko/20041107 Firefox/x.x";

        internal static async Task<HtmlDocument> GetDocument(string url)
        {
            Uri uri = new Uri(url);
            var req = WebRequest.Create(uri) as HttpWebRequest;

            // TODO:
            // Not sure if this bypasses the server cache.
            var environmentProxy = Environment.GetEnvironmentVariable("HTTP_PROXY", EnvironmentVariableTarget.User);

            if (!string.IsNullOrEmpty(environmentProxy))
            {
                req.Proxy = new WebProxy
                {
                    Address = new Uri(environmentProxy),
                    UseDefaultCredentials = true
                };
                req.UseDefaultCredentials = true;
            }
            req.UserAgent = _userAgent;
            req.Method = "GET";
            req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            var source = await req.GetResponseAsync();
            var stream = source.GetResponseStream();

            var doc = new HtmlDocument();
            doc.Load(stream, true);
            return await Task.FromResult(doc);
        }
    }
}