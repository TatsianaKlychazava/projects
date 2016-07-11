using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.Web.Models;

namespace ApotekaShop.Web.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly IProductDetailsService _productDetailsService;
        private readonly ConfigurationSettingsModel _configurationSettings;
        private readonly IWebContext _webContext;

        private int PageSize => _configurationSettings.DefaultPageSize;

        public ProductDetailsController(IProductDetailsService productDetailsService, IConfigurationSettingsProvider configurationSettingsProvider, IWebContext webContext)
        {
            _productDetailsService = productDetailsService;
            _configurationSettings = configurationSettingsProvider.GetConfiguration();
            _webContext = webContext;
        }

        // GET: SearchPage
        public async Task<ActionResult> Search(FilterOptionsViewModel filters)
        {
            int page = filters.PageNumber;

            if (page != 0)
            {
                page--;
            }

            if (filters.PageSize == 0)
            {
                filters.PageSize = PageSize;
            } 

            var filterModel = new FilterOptionsModel()
            {
                PageFrom = page,
                PageSize = filters.PageSize,
                MaxPrice = filters.MaxPrice * 100,
                MinPrice = filters.MinPrice * 100,
                Order = filters.Order,
                OrderBy = filters.OrderBy,
                LCID = (int)_webContext.GetCountry()
            };
            SearchResultModel result = await _productDetailsService.Search(filters.Query, filterModel);

            var model = new ProductDetailsViewModel
            {
                Products = result.Results,
                Total = result.TotalResults,
                PageCount = Convert.ToInt32(Math.Ceiling((double)(result.TotalResults / PageSize))),
                Filters = new FilterOptionsViewModel
                {
                    Query = filters.Query,
                    Order = filters.Order,
                    OrderBy = filters.OrderBy,
                    PageNumber = filters.PageNumber,
                    MaxPrice = filters.MaxPrice,
                    MinPrice = filters.MinPrice
                }
            };
            return View(model);
        }

        public async Task<JsonResult> Autocomplete(string query)
        {
            var results = await _productDetailsService.GetSuggestions(query);
            return Json(results.Select(r => new SuggestionViewModel(r, query)), JsonRequestBehavior.AllowGet);
        }

        // GET: SearchPage/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _productDetailsService.GetByPackageId(id));
        }
    }
}
