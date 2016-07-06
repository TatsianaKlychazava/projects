using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    /// <summary>
    /// Provide order processing functionality
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Get current user order items
        /// </summary>
        /// <returns>List of order items</returns>
        List<OrderItemModel> GetOrderItems();

        /// <summary>
        /// Add item to order items
        /// </summary>
        /// <param name="id">PackageId</param>
        /// <returns>Add result</returns>
        Task<object> AddOrderItem(int id);
        
        /// <summary>
        /// Update order items
        /// </summary>
        /// <param name="orderModel">Order items collection</param>
        /// <returns>Order items after update</returns>
        List<OrderItemModel> UpdateOrderItems(List<OrderItemModel> orderModel);

        /// <summary>
        /// Delete order item by id
        /// </summary>
        /// <param name="id">Order item Id(relevant to package id)</param>
        void DeleteOrderItem(int id);

        /// <summary>
        /// Update order item count
        /// </summary>
        /// <param name="id">Order item Id</param>
        /// <param name="count">New count value</param>
        void UpdateOrderItemCount(int id, int count);

        /// <summary>
        /// Get order
        /// </summary>
        /// <returns>Order model</returns>
        OrderModel GetOrder();

        /// <summary>
        /// Save order
        /// </summary>
        /// <param name="order">Order model</param>
        void SaveOrder(OrderModel order);
    }
}
