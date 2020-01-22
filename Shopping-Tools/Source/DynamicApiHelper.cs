using System;
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
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}