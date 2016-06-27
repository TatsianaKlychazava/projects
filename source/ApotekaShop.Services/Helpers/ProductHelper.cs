using System.Collections.Generic;
using System.Linq;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Helpers
{
    public static class ProductHelper
    {
        public static string GetFirstProductName(this IEnumerable<ProductName> products)
        {
            return products.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Name) == false)?.Name ?? string.Empty;
        }
        public static string GetAllProductNames(this IEnumerable<ProductName> products)
        {
            return string.Join(" ,", products.Where(x => string.IsNullOrWhiteSpace(x.Name) == false).Select(x => x.Name));
        }
    }
}
