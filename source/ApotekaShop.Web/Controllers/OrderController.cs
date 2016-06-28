using System.Threading.Tasks;
using System.Web.Mvc;
using ApotekaShop.Services.Interfaces;
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

        public async Task<JsonResult> AddItem(int id)
        {
            return Json(await _orderService.AddOrderItem(id), JsonRequestBehavior.AllowGet);
        }
    }
}