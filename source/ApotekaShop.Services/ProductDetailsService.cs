using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Nest;

namespace ApotekaShop.Services
{
    //Service are responsible for business logic related with product details
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly IProductDetailsElasticService _productDetailsElasticService;
        private readonly IWebContext _context;

        public ProductDetailsService(IProductDetailsElasticService productDetailsElasticService, IWebContext context)
        {
            _productDetailsElasticService = productDetailsElasticService;
            _context = context;
        }

        public async Task<ProductDetailsDTO> GetByPackageId(int id)
        {
            return await _productDetailsElasticService.GetByPackageId(id);
        }

        public async Task<BulkOperationResult> AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails)
        {
            return await _productDetailsElasticService.AddOrUpdate(productDetails);
        }

        public async Task<SearchResultModel> Search(string query, FilterOptionsModel filters)
        {
            var country = _context.GetCountry();
            filters.LCID = (int)country;
            return await _productDetailsElasticService.Search(query, filters);
        }

        public async Task<BulkOperationResult> ImportProductDetalils()
        {
            return await _productDetailsElasticService.ImportProductDetalils();
        }

        public async Task<bool> Delete(int id)
        {
            return await _productDetailsElasticService.Delete(id);
        }

        public async Task<bool> DeleteIndex()
        {
            return await _productDetailsElasticService.DeleteIndex();
        }
   }
}

