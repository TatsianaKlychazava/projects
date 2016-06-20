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
        private readonly IProductDetailsElasticService _productDetailsService;
        private const int PageSize = 10;

        public ProductDetailsController(IProductDetailsElasticService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        // GET: SearchPage
        public async Task<ActionResult> Search(FilterOptionsViewModel filters)
        {
            int page = filters.PageNumber;

            if (page != 0)
            {
                page--;
            }

            SearchResultModel result = await _productDetailsService.Search(filters.Query,
                new FilterOptionsModel()
                {
                    PageFrom = page,
                    PageSize = PageSize,
                    MaxPrice = filters.MaxPrice,
                    MinPrice = filters.MinPrice

                });

            var model = new ProductDetailsViewModel
            {
                Products = result.Results,
                Total = result.TotalResults,
                PageCount = Convert.ToInt32(Math.Ceiling((double) (result.TotalResults/PageSize))),
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
