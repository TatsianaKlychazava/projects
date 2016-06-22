using System.Web.Mvc;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly IWebContext _webContext;

        public CountryController(IWebContext webContext)
        {
            _webContext = webContext;
        }

        // GET: Culture
        public ActionResult Dropdown()
        {
            return PartialView(_webContext.GetCountry());
        }

        public RedirectResult SetCountry(Country country)
        { 
            _webContext.SetCountry(country);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}