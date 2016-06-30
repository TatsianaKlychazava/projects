using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services
{
    //Service are responsible for business logic related with product details
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly IProductDetailsElasticService _productDetailsElasticService;

        public ProductDetailsService(IProductDetailsElasticService productDetailsElasticService)
        {
            _productDetailsElasticService = productDetailsElasticService;
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

        public async Task<IEnumerable<string>> GetSuggestions(string query)
        {
            return await _productDetailsElasticService.GetSuggestions(query);
        }
   }
}

