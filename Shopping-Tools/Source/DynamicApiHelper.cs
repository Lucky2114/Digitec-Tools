using System;
using System.Linq;
using Shopping_Tools.Data.Enums;
using Shopping_Tools_Api_Services.Core.Brack;
using Shopping_Tools_Api_Services.Core.Digitec;

namespace Shopping_Tools.Source
{
    public static class DynamicApiHelper
    {
        public static string GetName(this Shops shop)
        {
            return shop.ToString();
        }

        public static T ShopNameToEnum<T>(string shopName)
        {
            return (T) Enum.Parse(typeof(T), shopName, true);
        }

        public static IApi GetInstance(this Shops shop)
        {
            //TODO Fix this:

            string strFullyQualifiedName = "Shopping_Tools_Api_Services.Core." + shop.ToString();
            try
            {
                var y = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                    .Where(x => typeof(IApi).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(x => x.Name).ToList();

            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
    }
}