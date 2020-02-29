using Microsoft.AspNetCore.Components.Authorization;
using Shopping_Tools.Source;
using Shopping_Tools_Api_Services.Core;
using Shopping_Tools_Api_Services.Models;
using System.Threading.Tasks;

namespace Shopping_Tools_Web.Source
{
    public static class Tools
    {
        public static async Task<RegisterProductResult> RegisterNewProduct(string productUrl, string email, AuthenticationStateProvider authenticationStateProvider, Storage storageInstance, IApi shop)
        {
            if (!shop.IsValidUrl(productUrl))
                return await Task.FromResult(RegisterProductResult.InvalidUrl);

            var product = await shop.GetProductInfoAsync(productUrl);
            var userData = new UserData() { Email = email, IPv4 = "not implemented" };

            var result = await storageInstance.AddNewProduct(product, userData, authenticationStateProvider);
            return result ? RegisterProductResult.Ok : RegisterProductResult.InternalError;
        }
    }
}