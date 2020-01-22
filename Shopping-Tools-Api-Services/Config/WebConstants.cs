using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Tools_Api_Services.Config
{
    internal static class DigitecWebConstants
    {
        internal const string ProductDetailClassName = "productDetail";
        internal const string ProductPriceOldClassName = "appendix";

        //Offsets
        internal const int ProductNameH1Index = 0;
        internal const int ProductPriceDivIndex = 0;
    }

    internal static class BrackWebConstants
    {
        internal const string ProductDetailClassName = "productStage__infoColumn";
        internal const string ProductManufacturerClassName = "productStage__itemManufacturer";
        internal const string ProductPricingSectionClassName = "productStage__pricingSection";
        internal const string ProductPriceClassName = "price";
        internal const string ProductOldPriceClassName = "regularPrice";
    }
}
