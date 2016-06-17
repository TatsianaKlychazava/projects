using System;
using System.Collections.Generic;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Models
{
    public class ProductDetailsViewModel
    {
        public IEnumerable<ProductDetailsDTO> Products { get; set; } 
        public FilterOptionsViewModel Filters { get; set; }
        public long Total { get; set; }
        public int PageCount { get; set; }
    }
}