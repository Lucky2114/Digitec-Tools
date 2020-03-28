using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class HttpHelper
    {
        private const string ScraperApiBaseUrl = "http://api.scraperapi.com/";

        internal static async Task<HtmlDocument> GetDocumentAsync(string url, bool fastRequest, int failedAttemps = 0)
        {
            if (fastRequest)
            {
                var response = await (new HttpClient().GetAsync(url));
                var html = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                return htmlDoc;
            }

            string apiKey = Environment.GetEnvironmentVariable("SCRAPERAPIKEY");
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("Environment variable 'SCRAPERAPIKEY' not set.");

            var requestUrl = ScraperApiBaseUrl + $"?api_key={apiKey}&url={url}";

            var request = new HttpClient();
            request.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true, NoStore = true };
            request.Timeout = TimeSpan.FromSeconds(60);

            var doc = new HtmlDocument();
            try
            {
                Console.WriteLine($"Starting ScraperApi Request: {requestUrl}");

                var response = await request.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var scrapedHtml = await response.Content.ReadAsStringAsync();
                doc.LoadHtml(scrapedHtml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SCRAPERAPI Limit reached! {ex.Message}");
            }
            return await Task.FromResult(doc);
        }
    }
}