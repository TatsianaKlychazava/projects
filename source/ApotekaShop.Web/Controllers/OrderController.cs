using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.Web.Models;

namespace ApotekaShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public ActionResult Details()
        {
            return View(new OrderViewModel {OrderItems = _orderService.GetOrderItems()});
        }

        [HttpPost]
        public ActionResult Details(OrderViewModel orderModel)
        {
            var model = new OrderViewModel {OrderItems = _orderService.UpdateOrderItems(orderModel.OrderItems)};

            return View(model);
        }

        // GET: Order
        public ActionResult Orders()
        {
            return PartialView(_orderService.GetOrderItems());
        }


        public ActionResult Checkout(string status)
        {
            var model = _orderService.GetOrder();

            if (!string.IsNullOrEmpty(status))
            {
                model.CurrentStep++;
                model.Status = status;
                _orderService.SaveOrder(model);
            }

            return View(model);
        }

        public void UpdateOrder(OrderModel order)
        {
            _orderService.SaveOrder(order);
        }

        public async Task<JsonResult> AddItem(int id)
        {
            return Json(await _orderService.AddOrderItem(id), JsonRequestBehavior.AllowGet);
        }

        public void DeleteItem(int id)
        {
            _orderService.DeleteOrderItem(id);
        }

        public void UpdateItemCount(int id, int count)
        {
            _orderService.UpdateOrderItemCount(id, count);
        }

        public JsonResult AggregatePaymentData()
        {
            var model = _orderService.GetOrder();
            if (model.CurrentStep != 1) return null;

            var baseUri = new UriBuilder(Request.Url.AbsoluteUri)
            {
                Path = Url.Action("Checkout", "Order"),
            }.Uri.ToString();

            object data = _paymentService.AggregatePaymentData(model.Items, baseUri);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}