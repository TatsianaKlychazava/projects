using System;
using System.Linq;
using System.Threading;
using System.Web;
using ApotekaShop.Services.Helpers;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Services
{
    public class WebContext: IWebContext
    {
        private readonly HttpContextBase _context;
        private const string CountryCookieName = "country";

        public WebContext(HttpContextBase context)
        {
            _context = context;
        }

        private bool? _isConnectionSecured;
        public bool IsConnectionSecured
        {
            get
            {
                if (_isConnectionSecured == null)
                {
                    _isConnectionSecured = false;
                    if (_context != null && _context.Request != null)
                    {
                        _isConnectionSecured = _context.Request.IsSecureConnection;
                        //Or maybe SSL offloaded on reverse-proxy
                        _isConnectionSecured |= _context.Request.Params.AllKeys.Any(x => string.Equals(x, "HTTP_X_ARR_SSL", StringComparison.InvariantCultureIgnoreCase));
                    }
                }

                return _isConnectionSecured.Value;
            }
        }

        private string _protocol = null;
        public string Protocol
        {
            get
            {
                if (string.IsNullOrEmpty(_protocol))
                    _protocol = IsConnectionSecured ? "https" : "http";
                return _protocol;
            }
        }

        private string _httpHost = null;
        public string HttpHost
        {
            get
            {
                if (string.IsNullOrEmpty(_httpHost))
                    _httpHost = GetServerVariables("HTTP_HOST");
                return _httpHost;
            }
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

        private string GetServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                if (_context?.Request == null)
                    return result;

                //should be in try-catch 
                //as described here http://www.nopcommerce.com/boards/t/21356/multi-store-roadmap-lets-discuss-update-done.aspx?p=6#90196
                if (_context.Request.ServerVariables[name] != null)
                {
                    result = _context.Request.ServerVariables[name];
                }
                if (string.IsNullOrWhiteSpace(result))
                    result = _context.Request.Url.DnsSafeHost;
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }
    }
}