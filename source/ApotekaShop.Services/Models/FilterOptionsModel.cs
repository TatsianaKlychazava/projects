namespace ApotekaShop.Services.Models
{
    public class FilterOptionsModel
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int PageFrom { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public Order? Order { get; set; }
    }

    public enum Order
    {
        Asc = 0,
        Desc = 1
    }
}
