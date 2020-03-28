using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shopping_Tools_Api_Services.Models;
using Newtonsoft.Json;

namespace Shopping_Tools.Source
{
    public static class Helpers
    {
        public static double ParseToDouble(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    sb.Append(c);
                }
            }
            if (string.IsNullOrEmpty(sb.ToString()))
                return 0;

            //The format "179.5.- has a point at the end.
            if (sb.ToString().Last() == '.')
                sb = sb.Remove(sb.ToString().Length -1, 1);
            return Convert.ToDouble(sb.ToString());
        }

        public static Product DictionaryToProduct(Dictionary<string, object> productDict)
        {
            var json = JsonConvert.SerializeObject(productDict);
            return JsonConvert.DeserializeObject<Product>(json);
        }
    }
}
