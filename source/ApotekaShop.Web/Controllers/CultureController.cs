using System.Globalization;
using System.Web.Mvc;
using ApotekaShop.Services;
using ApotekaShop.Web.Models;

namespace ApotekaShop.Web.Controllers
{
    public class CultureController : Controller
    {
        // GET: Culture
        public ActionResult Index()
        {
            return PartialView();
        }

        public RedirectResult SetCulture(string culture)
        { 
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}