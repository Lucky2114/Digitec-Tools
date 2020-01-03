using System.ComponentModel.DataAnnotations;

namespace Digitec_Tools_Web.Data
{
    public class PriceChangeModel
    {
        [Required]
        [Url]
        public string ProductUrl { get; set; }
    }
}
