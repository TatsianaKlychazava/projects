using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Nest;

namespace ApotekaShop.Services
{
    /// <summary>
    /// Use elasticsearch-2.3.3
    /// </summary>
    public class ProductDetailsElasticService : IProductDetailsService
    {
        private readonly ElasticClient _elasticClient;
        private readonly IProductDetailsDataProvider _productDetailsDataProvider;
        private const int DefaultSize = 10;

        public ProductDetailsElasticService(IProductDetailsDataProvider productDetailsDataProvider, Uri elasticNode, string defaultIndex)
        {
            _productDetailsDataProvider = productDetailsDataProvider;

            var settings = new ConnectionSettings(elasticNode);

            settings.DefaultIndex(defaultIndex);

            _elasticClient = new ElasticClient(settings);

            IClusterHealthResponse healthResponse = _elasticClient.ClusterHealth();

            if (healthResponse.ApiCall.Success == false) throw healthResponse.ApiCall.OriginalException;
        }

        public async Task<ProductDetailsDTO> GetByPackageId(int id)
        {
            var resGet = await _elasticClient.GetAsync<ProductDetailsDTO>(id);
            return resGet.Source;
        }

        public async Task AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails)
        {
            var descriptor = new BulkDescriptor();
            descriptor.IndexMany(productDetails, (indexDescriptor, dto) => indexDescriptor.Id(dto.PackageId));
            await _elasticClient.BulkAsync(descriptor);
        }

        public async Task<IEnumerable<ProductDetailsDTO>> Search(string query, FilterOptionsModel filters)
        {
            IQueryContainer queryContainer = CreateQuery(query, filters);

            var searchRequest = new SearchRequest()
            {
                From = filters.From,
                Size = filters.Size,
                Query = queryContainer as QueryContainer
            };

            ISearchResponse<ProductDetailsDTO> result = await _elasticClient.SearchAsync<ProductDetailsDTO>(searchRequest);

            return result.Documents;
        }

        /// <summary>
        /// Implementation for testing stage
        /// </summary>
        public async Task ImportProductDetalils()
        {
            List<ProductDetailsDTO> details = _productDetailsDataProvider.ImportProductDetalils();
            await AddOrUpdate(details);
        }

        public async Task Delete(int id)
        {
            await _elasticClient.DeleteAsync<ProductDetailsDTO>(id);
        }

        public async Task DeleteIndex()
        {
            await _elasticClient.DeleteIndexAsync(_elasticClient.ConnectionSettings.DefaultIndex);
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
            
            //set filter
            if (filter != null)
            {
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

                if (filter.Size == 0)
                {
                    filter.Size = DefaultSize;
                }
            }

            return queryContainer;
        }
    }
}
