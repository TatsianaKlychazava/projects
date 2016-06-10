using System;
using System.Collections.Generic;
using System.Text;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Nest;

namespace ApotekaShop.Services
{
    public class ProductDetailsElasticService : IProductDetailsService
    {
        private readonly ElasticClient _elasticClient;
        private const string DefaultIndex = "apatekashop-productdetails";

        public ProductDetailsElasticService(Uri elasticNode)
        {
            var settings = new ConnectionSettings(elasticNode);

            settings.DefaultIndex(DefaultIndex);

            _elasticClient = new ElasticClient(settings);
        }

        public ProductDetailsDTO GetByPackageId(int id)
        {
            IGetResponse<ProductDetailsDTO> resGet = _elasticClient.Get<ProductDetailsDTO>(id);
            return resGet.Source;
        }

        public void AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails)
        {
            foreach (var productDetail in productDetails)
            {
                _elasticClient.Index(productDetail, i => i.Id(productDetail.PackageId).Refresh());
            }
        }

        public IEnumerable<ProductDetailsDTO> Search(string query, FilterOptionsModel filters)
        {
            IQueryContainer queryContainer = CreateQuery(query, filters);
            
            var searchRequest = new SearchRequest()
            {
                Query = queryContainer as QueryContainer
            };

            ISearchResponse<ProductDetailsDTO> result = _elasticClient.Search<ProductDetailsDTO>(searchRequest);
         
            return result.Documents;
        }

        public void ImportProductDetalils()
        {
            List<ProductDetailsDTO> details = ProductDetailsDataProvider.ImportProductDetalils();

            AddOrUpdate(details);
        }

        private static IQueryContainer CreateQuery(string query, FilterOptionsModel filter)
        {
            QueryContainer queryContainer = null;

            if (!string.IsNullOrEmpty(query))
            {
                queryContainer &= new QueryStringQuery()
                {
                    Query = $"{query}*",
                    DefaultField = "_all"
                };
            }

            if (filter.MaxPrice > 0)
            {
                queryContainer &= new QueryContainerDescriptor<ProductDetailsDTO>()
                    .Range(r => r.LessThanOrEquals(filter.MaxPrice).Field(f => f.NormalizedPrice)
                );
            }

            if (filter.MinPrice > 0)
            {
                queryContainer &= new QueryContainerDescriptor<ProductDetailsDTO>()
                    .Range(r => r.GreaterThanOrEquals(filter.MinPrice).Field(f => f.NormalizedPrice)
                );
            }
            return queryContainer;
        }
    }
}
