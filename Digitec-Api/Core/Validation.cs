using System;
using System.Collections.Generic;
using System.Text;

namespace Digitec_Api.Core
{
    public static class Validation
    {
        public static bool IsValidDigitecUrl(string url)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch
            {
                //Url is not in the correct format
                return false;
            }

            return uri.Host.Equals("www.digitec.ch", StringComparison.OrdinalIgnoreCase) && uri.AbsolutePath.Contains("/product/");
        }
    }
}
