﻿using System;
using System.Collections.Generic;
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
        private const string DefaultIndex = "apatekashop-productdetails";

        public ProductDetailsElasticService(Uri elasticNode, string defaultIndex)
        {
            var settings = new ConnectionSettings(elasticNode);

            settings.DefaultIndex(defaultIndex);

            _elasticClient = new ElasticClient(settings);

            IClusterHealthResponse healthResponse = _elasticClient.ClusterHealth();
            
            if(healthResponse.ApiCall.Success == false) throw healthResponse.ApiCall.OriginalException;
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

        /// <summary>
        /// Implementation for testing stage
        /// </summary>
        public void ImportProductDetalils()
        {
            List<ProductDetailsDTO> details = ProductDetailsDataProvider.ImportProductDetalils();

            AddOrUpdate(details);
        }

        public void Delete(int id)
        {
            _elasticClient.Delete<ProductDetailsDTO>(id);
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
