using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Models
{
    public class OrderViewModel
    {
        public List<OrderItemModel> OrderItems { get; set; }
    }
}