using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Web.Controllers
{
    [RoutePrefix("api/ProductDetails")]
    public class ProductDetailsApiController : ApiController
    {
        private const string DONE = "Done";

        private readonly IProductDetailsElasticService _productDetailsService;
        private readonly ConfigurationSettingsModel _configSettings;

        public ProductDetailsApiController(IProductDetailsElasticService productDetailsService, IConfigurationSettingsProvider configSettingsProvider)
        {
            _productDetailsService = productDetailsService;
            _configSettings = configSettingsProvider.GetConfiguration();
        }

        /// <summary>
        /// Get product details by package id
        /// </summary>
        /// <param name="id">Package Id</param>
        /// <returns>Product details</returns>
        /// GET: api/ProductDetails/5
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByPackageId(int id)
        {
            ProductDetailsDTO productDetails = await _productDetailsService.GetByPackageId(id);

            if (productDetails == null)
            {
                return NotFound();
            }

            return Ok(productDetails);
        }

        /// <summary>
        /// Add or update product details collection
        /// </summary>
        /// <param name="productDetails">Product details collection</param>
        /// <returns>Ok if data added or updated</returns>
        /// POST: api/ProductDetails
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddOrUpdate([FromBody]List<ProductDetailsDTO> productDetails)
        {
            if (productDetails == null || !productDetails.Any()) return BadRequest();

            try
            {
                BulkOperationResult result = await _productDetailsService.AddOrUpdate(productDetails);
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        /// <summary>
        /// Delete product details by package id
        /// </summary>
        /// <param name="id">Package Id</param>
        /// <returns>Ok if data removed</returns>
        // DELETE: api/ProductDetails/5
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var result = await _productDetailsService.Delete(id);

                if (result)
                {
                    return Ok(DONE);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Search and sort product details by query and filters
        /// </summary>
        /// <param name="query">Query string for search</param>
        /// <param name="filters">Options for filtering, sorting, paging</param>
        /// <returns>Search result model</returns>
        /// GET: api/ProductDetails/Search?query={query for search}&minPrice={filter for min price}&maxPrice={filter for max price}&pageFrom={page number}&pageSize={page size}&orderBy={order name}&order={sorting direction}
        [Route("Search")]
        [HttpGet]
        public async Task<IHttpActionResult> Search([FromUri]string query, [FromUri]FilterOptionsModel filters)
        {
            if (string.IsNullOrEmpty(query) || query.Length < _configSettings.MinQueryLength)
            {
                return BadRequest($"Query string contains less than {_configSettings.MinQueryLength} characters");
            }

            if (!string.IsNullOrEmpty(filters.OrderBy) && !_configSettings.FilterOptions.ContainsKey(filters.OrderBy.ToLower()))
            {
                return BadRequest("Incorrect orderBy value");
            }

            var result = (await _productDetailsService.Search(query, filters));

            if (result.Results.Count() != 0)
            {
                return Ok(result);
            }

            return NotFound();
        }    
    }
}
