using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        private const string ScraperApiBaseUrl = "http://api.scraperapi.com/";

        internal static async Task<HtmlDocument> GetDocumentAsync(string url, bool fastRequest, int failedAttemps = 0)
        {
            //string apiKey = Environment.GetEnvironmentVariable("SCRAPERAPIKEY");
            string apiKey = "5f1c929bc06a341c9752b18a361cdc4f";
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("Environment variable 'SCRAPERAPIKEY' not set.");

            var requestUrl = ScraperApiBaseUrl + $"?api_key={apiKey}&url={url}";

            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Timeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;

            var doc = new HtmlDocument();
            try
            {
                Console.WriteLine($"Starting ScraperApi Request: {requestUrl}");

                var response = await request.GetResponseAsync();
                var scrapedHtml = response.GetResponseStream();
                doc.Load(scrapedHtml);
            }
            catch (WebException ex)
            {
                Console.WriteLine($"SCRAPERAPI Limit reached! {ex.Message}");
            }
            return await Task.FromResult(doc);
        }

        private static WebHeaderCollection GetHeaderCollection(string url)
        {
            var uri = new Uri(url);
            return new WebHeaderCollection
            {
                { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" },
                {  "Accept-Encoding", "gzip, deflate, br"},
                { "Accept-Language", "de-CH,de-DE;q=0.9,de;q=0.8,en-US;q=0.7,en;q=0.6"},
                { "Cache-Control", "max-age=0"},
                { "Host", $"{uri.Host}" },
                { "Referer", "https://www.google.ch"},
                { "Sec-Fetch-Mode", "navigate"},
                {"Sec-Fetch-Site", "cross-site" },
                { "Sec-Fetch-User", "?1"},
                { "Upgrade-Insecure-Requests", "1" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36"},
            };
        }
    }
}