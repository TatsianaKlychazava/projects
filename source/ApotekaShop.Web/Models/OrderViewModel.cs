using System.Collections.Generic;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Models
{
    public class OrderViewModel
    {
        public List<OrderItemModel> OrderItems { get; set; }
    }
}