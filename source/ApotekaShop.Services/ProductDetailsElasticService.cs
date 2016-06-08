using System;
using System.Collections.Generic;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Nest;

namespace ApotekaShop.Services
{
    public class ProductDetailsElasticService: IProductDetailsService
    {
        private ElasticClient _elasticClient;

        public ProductDetailsElasticService()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);

            settings.DefaultIndex("apatekashop-productdetails");

            _elasticClient = new ElasticClient(settings);
        }

        public ProductDetailsDTO GetByPackageId(int id)
        {
            throw new System.NotImplementedException();
        }

        public void AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails)
        {
            throw new System.NotImplementedException();
        }
    }
}
