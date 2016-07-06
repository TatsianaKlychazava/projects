﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApotekaShop.Services.Helpers;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Services
{
    public class OrderSessionService : IOrderService
    {
        private readonly IProductDetailsService _productDetailsService;

        private const string OrderItemsKey = "orderItems";
        private const string OrderKey = "order";


        public OrderSessionService(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        public OrderModel GetOrder()
        {

            //todo need additional requirements from ux guy
            /*if (HttpContext.Current.Session[OrderKey] == null)
            {
                return new OrderModel()
                {
                    Items = GetOrderItems(),
                    CurrentStep = 0
                };
            }
            return HttpContext.Current.Session[OrderKey] as OrderModel;*/
            return new OrderModel()
            {
                Items = GetOrderItems(),
                CurrentStep = 0
            };
        }
        
        public void SaveOrder(OrderModel order)
        {
            HttpContext.Current.Session[OrderKey] = order;
        }

        public List<OrderItemModel> GetOrderItems()
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

        public void DeleteOrderItem(int id)
        {
            var orderItems = GetOrderItems();

            orderItems.Remove(orderItems.FirstOrDefault(item => item.Id == id));

            HttpContext.Current.Session[OrderItemsKey] = orderItems;
        }

        public void UpdateOrderItemCount(int id, int count)
        {
            var orderItems = GetOrderItems().ToList();
            var item = orderItems.FirstOrDefault(o => o.Id == id);

            if (item != null)
            {
                item.Count = count;
            }
            
            HttpContext.Current.Session[OrderItemsKey] = orderItems;
        }

        public List<OrderItemModel> UpdateOrderItems(List<OrderItemModel> orderItems)
        {
            var unmodifiedOrderItems = GetOrderItems().ToList();
            foreach (var orderItem in orderItems)
            {
                var item = unmodifiedOrderItems.FirstOrDefault(o => o.Id == orderItem.Id);

                if (item != null)
                {
                    item.Count = orderItem.Count;
                }
            }

            HttpContext.Current.Session[OrderItemsKey] = unmodifiedOrderItems;
            return unmodifiedOrderItems;
        }
    }
}
