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
        private readonly ConfigurationSettingsModel _configurationSettings;

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
            IGetResponse<ProductDetailsDTO> response = await _elasticClient.GetAsync<ProductDetailsDTO>(id);
            return response.Source;
        }

        public async Task<BulkOperationResult> AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails)
        {
            var descriptor = new BulkDescriptor();

            descriptor.Refresh(true);
            descriptor.IndexMany(productDetails, (indexDescriptor, dto) => indexDescriptor.Id(dto.PackageId));

            IBulkResponse response = await _elasticClient.BulkAsync(descriptor);

            return new BulkOperationResult { HasErrors = response.Errors, ProcessedCount = response.Items.Count(), TookMilliseconds = response.Took };
        }

        public async Task<SearchResultModel> Search(string query, FilterOptionsModel filters)
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

            return new SearchResultModel()
            {
                Results = result.Documents,
                TotalResults = result.Total,
                ExecutionTime = result.Took
            };
        }

        private List<ISort> CreateSortFields(FilterOptionsModel filter)
        {
            if (string.IsNullOrEmpty(filter.OrderBy) ||
                filter.Order == null ||
                !_configurationSettings.FilterOptions.ContainsKey(filter.OrderBy.ToLower())) return null;

            var field = _configurationSettings.FilterOptions[filter.OrderBy.ToLower()];
            //Ensure field name is lower camel
            field = Char.ToLowerInvariant(field[0]) + field.Substring(1);
            var sorts = new List<ISort>
            {
                new SortField
                {
                    Order = (SortOrder)filter.Order,
                    Field = field
                }
            };

            return sorts;
        }
    
        public async Task<BulkOperationResult> ImportProductDetalils()
        {
            List<ProductDetailsDTO> details = _productDetailsDataProvider.ImportProductDetalils();
            return await AddOrUpdate(details);
        }

        public async Task<bool> Delete(int id)
        {
            IDeleteResponse deleteResponse = await _elasticClient.DeleteAsync<ProductDetailsDTO>(id);

            return deleteResponse.Found;
        }

        public async Task<bool> DeleteIndex()
        {
            IDeleteIndexResponse deleteResponse =
                await _elasticClient.DeleteIndexAsync(_elasticClient.ConnectionSettings.DefaultIndex);
            return deleteResponse.IsValid;
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
                    DefaultOperator = Operator.And

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
