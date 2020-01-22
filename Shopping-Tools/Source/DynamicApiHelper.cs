using System;
using Shopping_Tools.Data.Enums;
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
            try
            {
                var instance = Activator.CreateInstance(Type.GetType(shop.ToString()));
                return (IApi) instance;
            }
            catch
            {
                Console.WriteLine("Failed to instantiate class. Wrong class name maybe?");
            }
            return null;
        }
    }
}