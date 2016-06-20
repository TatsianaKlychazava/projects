using ApotekaShop.Services.Interfaces;

namespace ApotekaShop.Services
{
    //Service are responsible for business logic related with product details
    public class ProductDetailsService: IProductDetailsService
    {
        private readonly IProductDetailsElasticService _productDetailsElasticService;

        public ProductDetailsService(IProductDetailsElasticService productDetailsElasticService)
        {
            _productDetailsElasticService = productDetailsElasticService;
        }


    }
}
