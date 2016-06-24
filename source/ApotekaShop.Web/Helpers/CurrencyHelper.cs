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
            if (!price.HasValue) return string.Empty;
            decimal currentPrice = price.Value/100;
            return onlyPrice ? currentPrice.ToString("F2", CultureInfo.InvariantCulture) : String.Format("{0} {1}", ShopResources.CurrencyName, currentPrice.ToString("F2",CultureInfo.InvariantCulture));
        }
    }
}