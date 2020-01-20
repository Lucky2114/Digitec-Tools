using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Tools_Api_Services.Core
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
