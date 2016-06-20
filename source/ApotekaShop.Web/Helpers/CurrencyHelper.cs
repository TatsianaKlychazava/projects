using System;
using System.Web.Mvc;

namespace ApotekaShop.Web.Helpers
{
    public static class CurrencyHelper
    {
        public static string Currency(this HtmlHelper helper, int? price)
        {
            if (!price.HasValue) return string.Empty;

            return String.Format("DKK {0}", price.Value.ToString("N2"));
        }
    }
}