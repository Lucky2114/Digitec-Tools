using System.Threading.Tasks;
using Digitec_Api;
using Digitec_Api.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Digitec_Tools_Web.Source
{
    public static class Tools
    {
        public static async Task<bool> RegisterNewProduct(string productUrl, string email, AuthenticationStateProvider authenticationStateProvider)
        {
            var product = await Digitec.GetProductInfo(productUrl);
            var userData = new UserData() { Email = email, IPv4 = "not implemented" };

            var storage = Storage.GetInstance(authenticationStateProvider);
            return await storage.AddNewProduct(product, userData);
        }
    }
}