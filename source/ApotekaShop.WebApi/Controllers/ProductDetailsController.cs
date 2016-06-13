using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ApotekaShop.Services.Models;
using ApotekaShop.Services.Interfaces;

namespace ApotekaShop.WebApi.Controllers
{
    [RoutePrefix("api/ProductDetails")]
    public class ProductDetailsController : ApiController
    {
        private const string DONE = "Done";

        private readonly IProductDetailsService _productDetailsService;

        public ProductDetailsController(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        // GET: api/ProductDetails/5
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

        // POST: api/ProductDetails
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> AddOrUpdate([FromBody]IEnumerable<ProductDetailsDTO> productDetails)
        {
            if (productDetails == null || !productDetails.Any()) return BadRequest();

            try
            {
                await _productDetailsService.AddOrUpdate(productDetails);
                return Ok(DONE);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        // DELETE: api/ProductDetails/5
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                await _productDetailsService.Delete(id);
                return Ok(DONE);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/ProductDetails/Search
        [Route("Search")]
        [HttpGet]
        public async Task<IHttpActionResult> Search([FromUri]string query = "", [FromUri]FilterOptionsModel filters = null)
        {
            List<ProductDetailsDTO> result = (await _productDetailsService.Search(query, filters)).ToList();

            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        } 

        //For tests
        [Route("ImportIndex")]
        [HttpGet]
        public async Task<IHttpActionResult> ImportIndex()
        {
            try
            {
                await _productDetailsService.ImportProductDetalils();
                return Ok(DONE);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("RemoveIndex")]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveIndex()
        {
            try
            {
                await _productDetailsService.DeleteIndex();
                return Ok(DONE);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
