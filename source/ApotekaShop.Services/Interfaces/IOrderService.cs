using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderItemModel> GetOrderItems();
        Task<object> AddOrderItem(int id);
        List<OrderItemModel> UpdateOrderItems(List<OrderItemModel> orderModel);
        void DeleteOrderItem(int id);
        void UpdateOrderItemCount(int id, int count);
    }
}
