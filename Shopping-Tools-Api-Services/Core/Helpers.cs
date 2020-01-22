using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shopping_Tools_Api_Services.Core
{
    internal static class Helpers
    {
        internal static string ExtractIdFromUrl(string url)
        {
            return url.Split("-").Last().Split("?").First();
        }

        internal static Uri RemoveParameters(this Uri uri)
        {
            var path = uri.ToString().Split("?").First();
            return new Uri(path);
        }
    }
}