using Digitec_Api.Config;
using Digitec_Api.Core;
using System;
using System.Linq;

namespace Digitec_Api
{
    public static class Digitec
    {
        public static async void GetProductInfo(string productUrl)
        {
            if (Validation.IsValidDigitecUrl(productUrl))
            {
                var doc = await HttpHelper.GetDocument(productUrl);
                var x = doc.DocumentNode.Descendants().Where(x => x.GetClasses().SequenceEqual(DigitecWebConstatnts.ProductElementClasses));
            }
        }
    }
}
