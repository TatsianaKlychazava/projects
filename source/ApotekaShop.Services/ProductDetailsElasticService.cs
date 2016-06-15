using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Nest;
using SortOrder = Nest.SortOrder;

namespace ApotekaShop.Services
{
    /// <summary>
    /// Use elasticsearch-2.3.3
    /// </summary>
    public class ProductDetailsElasticService : IProductDetailsService
    {
        private readonly ElasticClient _elasticClient;
        private readonly IProductDetailsDataProvider _productDetailsDataProvider;
        private ConfigurationSettingsModel _configurationSettings;

        public ProductDetailsElasticService(IProductDetailsDataProvider productDetailsDataProvider, IConfigurationSettingsProvider configurationSettingsProvider)
        {
            _productDetailsDataProvider = productDetailsDataProvider;

            _configurationSettings = configurationSettingsProvider.GetConfiguration();

            var settings = new ConnectionSettings(_configurationSettings.ElasticNodeUrl);

            settings.DefaultIndex(_configurationSettings.DefaultIndex);

            _elasticClient = new ElasticClient(settings);

            IClusterHealthResponse healthResponse = _elasticClient.ClusterHealth();

            if (healthResponse.ApiCall.Success == false) throw healthResponse.ApiCall.OriginalException;
        }

        public async Task<ProductDetailsDTO> GetByPackageId(int id)
        {
            var resGet = await _elasticClient.GetAsync<ProductDetailsDTO>(id);
            return resGet.Source;
        }

        public async Task<ElasticBulkOperationResult> AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails)
        {
            var descriptor = new BulkDescriptor();
            descriptor.Refresh(true);
            descriptor.IndexMany(productDetails, (indexDescriptor, dto) => indexDescriptor.Id(dto.PackageId));
            var resp = await _elasticClient.BulkAsync(descriptor);
            return new ElasticBulkOperationResult { HasErrors = resp.Errors, ProcessedCount = resp.Items.Count(), TookMilliseconds = resp.Took };
        }

        public async Task<IEnumerable<ProductDetailsDTO>> Search(string query, FilterOptionsModel filters)
        {
            IQueryContainer queryContainer = CreateQuery(query, filters);
            List<ISort> sortFields = CreateSortFields(filters);
            var searchRequest = new SearchRequest<ProductDetailsDTO>()
            {
                From = filters.PageFrom * filters.PageSize,
                Size = filters.PageSize,
                Query = queryContainer as QueryContainer,
                Sort = sortFields
            };

            ISearchResponse<ProductDetailsDTO> result = await _elasticClient.SearchAsync<ProductDetailsDTO>(searchRequest);
            
            return result.Documents;
        }

        private List<ISort> CreateSortFields(FilterOptionsModel filter)
        {
            if (string.IsNullOrEmpty(filter.OrderBy) ||
                filter.Order == null ||
                !_configurationSettings.FilterOptions.ContainsKey(filter.OrderBy.ToLower())) return null;

            var sorts = new List<ISort>()
            {
                new SortField()
                {
                    Order = (SortOrder)filter.Order,
                    Field = _configurationSettings.FilterOptions[filter.OrderBy.ToLower()]  
                }
            };

            return sorts;
        }
    
        /// <summary>
        /// Implementation for testing stage
        /// </summary>
        public async Task<ElasticBulkOperationResult> ImportProductDetalils()
        {
            List<ProductDetailsDTO> details = _productDetailsDataProvider.ImportProductDetalils();
            return await AddOrUpdate(details);
        }

        public async Task Delete(int id)
        {
            await _elasticClient.DeleteAsync<ProductDetailsDTO>(id);
        }

        public async Task DeleteIndex()
        {
            await _elasticClient.DeleteIndexAsync(_elasticClient.ConnectionSettings.DefaultIndex);
        }

        private IQueryContainer CreateQuery(string query, FilterOptionsModel filter)
        {
            QueryContainer queryContainer = null;

            if (!string.IsNullOrEmpty(query))
            {
                queryContainer &= new QueryStringQuery()
                {
                    Query = $"{query}",
                    DefaultField = "_all",
                    DefaultOperator = Operator.And,

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

                if (filter.PageSize == 0)
                {
                    filter.PageSize = _configurationSettings.DefaultPageSize;
                }
            }

            return queryContainer;
        }
    }
}
