using System;
using System.Web.Mvc;
using ApotekaShop.Web.App_LocalResources;

namespace ApotekaShop.Web.Helpers
{
    public static class CurrencyHelper
    {
        public static string Currency(this HtmlHelper helper, int? price)
        {
            if (!price.HasValue) return string.Empty;
            decimal currentPrice = price.Value/100;
            return String.Format("{0} {1}", ShopResources.CurrencyName, currentPrice.ToString("N2"));
        }
    }
}