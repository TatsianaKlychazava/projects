using System;
using System.Globalization;
using System.Web.Mvc;
using Resources;

namespace ApotekaShop.Web.Helpers
{
    public static class CurrencyHelper
    {
        public static string Currency(this HtmlHelper helper, int? price, bool onlyPrice = false)
        {
            if (!price.HasValue) return "N/A";
            double currentPrice = price.Value / 100.00;
            return onlyPrice ? currentPrice.ToString("F2", CultureInfo.InvariantCulture) :
                $"{ShopResources.CurrencyName} {currentPrice.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}