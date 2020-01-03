using System.Threading.Tasks;
using Digitec_Api;
using Digitec_Api.Models;

namespace Digitec_Tools_Web.Source
{
    public static class Tools
    {
        public static async Task<bool> RegisterNewProduct(string productUrl, string email)
        {
            var _product = await Digitec.GetProductInfo(productUrl);
            var userData = new UserData() { Email = email, IPv4 = "not implemented" };

            var _storage = new Storage();
            return await _storage.AddNewProduct(_product, userData);
        }
    }
}