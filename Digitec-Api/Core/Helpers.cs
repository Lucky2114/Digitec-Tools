using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digitec_Api.Core
{
    internal static class Helpers
    {
        internal static string ExtractIdFromUrl(string url)
        {
            if (Validation.IsValidDigitecUrl(url))
            {
                return url.Split("-").Last();
            }
            return "";
        }
    }
}
