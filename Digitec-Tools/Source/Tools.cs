using System.Threading.Tasks;
using Digitec_Api;

namespace Digitec_Tools.Source
{
    public static class Tools
    {
        public static async Task RegisterNewProduct(string productUrl)
        {
            //Every Product is a key.
            //each Product has a list of email addresses.
            
            //TODO Add Data Storage
            var _product = await Digitec.GetProductInfo(productUrl);
        }
    }
}