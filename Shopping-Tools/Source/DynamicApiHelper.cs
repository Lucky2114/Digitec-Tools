using Shopping_Tools.Data.Enums;

namespace Shopping_Tools.Source
{
    public static class DynamicApiHelper
    {
        public static string GetName(this Shops shop)
        {
            return shop switch
            {
                Shops.Digitec => "Digitec",
                _ => ""
            };
        }

        public static Shops? ShopNameToEnum(string shopName)
        {
            switch (shopName)
            {
                case "Digitec":
                    return Shops.Digitec;
            }
            return null;
        }
    }
}