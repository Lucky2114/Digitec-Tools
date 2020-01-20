using System.ComponentModel.DataAnnotations;

namespace Shopping_Tools_Web.Data
{
    public class RegisterProductModel
    {
        [Required]
        [Url]
        public string ProductUrl { get; set; }
    }
}
