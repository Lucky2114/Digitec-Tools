using System.Threading.Tasks;
using Shopping_Tools_Api_Services;
using Shopping_Tools_Api_Services.Models;
using Shopping_Tools.Source;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping_Tools.Data.Enums;
using Shopping_Tools_Api_Services.Core.Digitec;

namespace Shopping_Tools_Web.Source
{
    public static class Tools
    {
        public static async Task<bool> RegisterNewProduct(string productUrl, string email, AuthenticationStateProvider authenticationStateProvider, Storage storageInstance, Shops shop)
        {
            var product = await new Digitec().GetProductInfo(productUrl);
            var userData = new UserData() { Email = email, IPv4 = "not implemented" };

            //var storage = Storage.GetInstance(authenticationStateProvider, DynamicApiHelper.ShopNameToEnum(product.OnlineShopName));
            return await storageInstance.AddNewProduct(product, userData, authenticationStateProvider);
        }
    }
}