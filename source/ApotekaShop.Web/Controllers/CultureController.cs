using System.Globalization;
using System.Web.Mvc;
using ApotekaShop.Web.Models;

namespace ApotekaShop.Web.Controllers
{
    public class CultureController : Controller
    {
        // GET: Culture
        public ActionResult Index()
        {
            return PartialView(new CultureViewModel() {SelectedCulture = CultureInfo.CurrentCulture});
        }

        public RedirectResult SetCulture(string culture)
        { 
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}