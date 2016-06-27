using System.Threading.Tasks;
using System.Web.Mvc;
using ApotekaShop.Services.Interfaces;

namespace ApotekaShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public ActionResult Index()
        {
            return View(_orderService.GetOrderItems());
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