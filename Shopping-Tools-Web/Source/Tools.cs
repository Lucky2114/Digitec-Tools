using System.Threading.Tasks;
using Shopping_Tools_Api_Services;
using Shopping_Tools_Api_Services.Models;
using Shopping_Tools.Source;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping_Tools_Api_Services.Core.Digitec;

namespace Shopping_Tools_Web.Source
{
    public static class Tools
    {
        public static async Task<bool> RegisterNewProduct(string productUrl, string email, AuthenticationStateProvider authenticationStateProvider, Storage storageInstance, IApi shop)
        {
            var product = await shop.GetProductInfo(productUrl);
            var userData = new UserData() { Email = email, IPv4 = "not implemented" };

            return await storageInstance.AddNewProduct(product, userData, authenticationStateProvider);
        }
    }
}