using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ApotekaShop.Services.Models;
using ApotekaShop.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace ApotekaShop.WebApi.Controllers
{
    [RoutePrefix("api/ProductDetails")]
    public class ProductDetailsController : ApiController
    {
        private readonly IProductDetailsService _productDetailsService;

        public ProductDetailsController(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        // GET: api/ProductDetails/5
        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult GetByPackageId(int id)
        {
            ProductDetailsDTO productDetails = _productDetailsService.GetByPackageId(id);

            if (productDetails == null)
            {
                return NotFound();
            }

            return Ok(productDetails);
        }

        // POST: api/ProductDetails
        [Route("")]
        [HttpPost]
        public IHttpActionResult AddOrUpdate([FromBody]IEnumerable<ProductDetailsDTO> productDetails)
        {
            try
            {
                _productDetailsService.AddOrUpdate(productDetails);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        // DELETE: api/ProductDetails/5
        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _productDetailsService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // GET: api/ProductDetails/Search
        [Route("Search")]
        [HttpGet]
        public IHttpActionResult Search([FromUri]string query = "", [FromUri]FilterOptionsModel filters = null)
        {
            List<ProductDetailsDTO> result = _productDetailsService.Search(query, filters).ToList();

            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        } 

        //For tests
        [Route("ImportIndex")]
        [HttpGet]
        public IHttpActionResult ImportIndex()
        {
            try
            {
                _productDetailsService.ImportProductDetalils();
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("RemoveIndex")]
        [HttpGet]
        public IHttpActionResult RemoveIndex()
        {
            try
            {
                _productDetailsService.RemoveIndex();
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
