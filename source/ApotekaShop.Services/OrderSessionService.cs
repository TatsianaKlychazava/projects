using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApotekaShop.Services.Helpers;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Nest;

namespace ApotekaShop.Services
{
    public class OrderSessionService : IOrderService
    {
        private readonly IProductDetailsService _productDetailsService;

        private const string OrderItemsKey = "orderItems";

        public OrderSessionService(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        public IEnumerable<OrderItemModel> GetOrderItems()
        {
            if (HttpContext.Current.Session[OrderItemsKey] == null)
            {
                return new List<OrderItemModel>();
            }
            
            return HttpContext.Current.Session[OrderItemsKey] as List<OrderItemModel>;
        }

        public async Task<object> AddOrderItem(int id)
        {
            ProductDetailsDTO product = await _productDetailsService.GetByPackageId(id);

            if (product.NormalizedPrice == null)
                return new {status = "Faild", message = "Can't add item without price"};

            var orderItems = GetOrderItems().ToList();

            var orderItem = orderItems.FirstOrDefault(item => item.Id == id);

            if (orderItem != null)
            {
                orderItem.Count++;
            }
            else
            {
                orderItems.Add(new OrderItemModel()
                {
                    Count = 1,
                    Id = id,
                    Name = product.ProductNames.GetFirstProductName(),
                    PricePerUnit = (int)product.NormalizedPrice
                });
            }

            HttpContext.Current.Session[OrderItemsKey] = orderItems;
            
            return new {status = "Done", count = orderItems.Count};
        }
    }
}
