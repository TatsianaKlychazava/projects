using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderItemModel> GetOrderItems();
        Task<object> AddOrderItem(int id);
    }
}
