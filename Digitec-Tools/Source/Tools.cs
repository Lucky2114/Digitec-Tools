using System.Threading.Tasks;
using Digitec_Api;
using Digitec_Api.Models;

namespace Digitec_Tools.Source
{
    public static class Tools
    {
        public static async Task RegisterNewProduct(string productUrl, string email)
        {
            var _product = await Digitec.GetProductInfo(productUrl);
            var userData = new UserData() { Email = email, IPv4 = "not implemented" };

            await new Storage().AddNewProduct(_product, userData);
        }
    }
}