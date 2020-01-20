using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Tools_Api_Services.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string PriceCurrent { get; set; }
        public string PriceOld { get; set; }
        public string ProductId { get; set; }
        public string ProductIdSimple { get; set; }
        public string Url { get; set; }
    }
}
