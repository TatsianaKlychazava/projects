using System;
using System.Globalization;
using System.Threading;
using System.Web;
using ApotekaShop.Services.Helpers;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web
{
    public class WebContext: IWebContext
    {
        private readonly HttpContextBase _context;
        private const string CountryCookieName = "country";

        public WebContext(HttpContextBase context)
        {
            _context = context;
        }

        public void SetCountry(Country country)
        {
            var httpCookie = new HttpCookie(CountryCookieName, country.ToString());

            _context.Response.Cookies.Set(httpCookie);

            //set culture for selected country
            var currentCulture = country.ToCulture();
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        public Country GetCountry()
        {
            var country = Country.DK;

            if (!string.IsNullOrEmpty(_context.Request.Cookies[CountryCookieName]?.Value))
            {
                Enum.TryParse(_context.Request.Cookies[CountryCookieName].Value, true, out country);
            }

            return country;
        }
    }
}