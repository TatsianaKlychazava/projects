using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using ApotekaShop.Services;
using ApotekaShop.Services.Models;
using ApotekaShop.Services.Interfaces;

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
        [Route("")]
        public void Delete(int id)
        {

        }

        // GET: api/ProductDetails/Search
        [Route("Search")]
        [HttpGet]
        public IEnumerable<ProductDetailsDTO> Search([FromUri]string query = "", [FromUri]FilterOptionsModel filters = null)
        {
            return _productDetailsService.Search(query, filters);
        } 

        //For tests
        [Route("Import")]
        [HttpGet]
        public void ImportIndex()
        {
            _productDetailsService.ImportProductDetalils();
        }
    }
}
