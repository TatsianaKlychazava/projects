using System;
using System.Web.Mvc;

namespace ApotekaShop.Web.Helpers
{
    public static class CurrencyHelper
    {
        public static string Currency(this HtmlHelper helper, int? price)
        {
            if (!price.HasValue) return string.Empty;
            decimal currentPrice = price.Value/100;
            return String.Format("DKK {0}", currentPrice.ToString("N2"));
        }
    }
}