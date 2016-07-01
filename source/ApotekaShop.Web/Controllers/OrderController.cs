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

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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


        public ActionResult Checkout()
        {
            var model = new OrderModel
            {
                Items = _orderService.GetOrderItems(),
                CurrentStep = string.Empty
            };

            return View(model);
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

    }
}