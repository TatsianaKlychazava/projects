using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.Web.Models;

namespace ApotekaShop.Web.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly IProductDetailsService _productDetailsService;
        private readonly ConfigurationSettingsModel _config;
        private const int PageSize = 10;
        public ProductDetailsController(IProductDetailsService productDetailsService, IConfigurationSettingsProvider congiSettingsProvider)
        {
            _productDetailsService = productDetailsService;
            _config = congiSettingsProvider.GetConfiguration();
        }

        // GET: SearchPage
        public async Task<ActionResult> Search(FilterOptionsViewModel filters)
        {
            SearchResultModel result = await _productDetailsService.Search(filters.Query, 
                new FilterOptionsModel()
                {
                    PageFrom = filters.PageNumber - 1,
                    PageSize = PageSize,
                    MaxPrice = filters.MaxPrice,
                    MinPrice = filters.MinPrice

                });

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
                    PageNumber = filters.PageNumber               
                }
            };

            return View(model);
        }

        // GET: SearchPage/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _productDetailsService.GetByPackageId(id));
        }
    }
}
