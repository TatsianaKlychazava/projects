using System.Collections.Generic;

namespace ApotekaShop.Services.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int CurrentStep { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string BillingAddress { get; set; }
        public string Email { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string VoucherCode { get; set; }
        public string MobileNumber { get; set; }
        public IEnumerable<OrderItemModel> Items { get; set; }
        public string Status { get; set; }
    }
}
