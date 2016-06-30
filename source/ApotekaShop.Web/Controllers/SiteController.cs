using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Controllers
{
    public class SiteController : Controller
    {
        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private readonly IWebContext _webContext;
        private readonly IProductDetailsService _productDetailsService;

        private string Protocol => _webContext.Protocol;
        private string HttpHost => _webContext.HttpHost;

        public SiteController(IProductDetailsService productDetailsService, IWebContext webContext)
        {
            _productDetailsService = productDetailsService;
            _webContext = webContext;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        public async Task<ActionResult> SitemapXml()
        {
            XNamespace xmlns = SitemapsNamespace;

            var result = await _productDetailsService.Search(string.Empty, new FilterOptionsModel { PageSize = 10000 });
            var root = new XElement(xmlns + "urlset");
            var doc = new XDocument(new XDeclaration("1.0", Encoding.UTF8.WebName, null), root);


            foreach (var item in result.Results)
            {
                doc.Root.Add(
                    new XElement(xmlns + "url",
                        new XElement(xmlns + "loc",
                            Uri.EscapeUriString($"{Protocol}://{HttpHost}/ProductDetails/Details/{item.PackageId}"))
                        //,new XElement(xmlns + "lastmod", item.CreatedDate.ToString("yyyy-MM-dd"))
                        ));
            }

            return Content(doc.Declaration + doc.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}