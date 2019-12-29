using System.ComponentModel.DataAnnotations;
namespace Digitec_Tools.Data
{
    public class PriceChangeModel
    {
        [Required]
        [Url]
        public string ProductUrl { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
