using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Models
{
    public class FilterOptionsViewModel
    {
        public string Query { get; set; }
        public Order Order { get; set; }
        public string OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}