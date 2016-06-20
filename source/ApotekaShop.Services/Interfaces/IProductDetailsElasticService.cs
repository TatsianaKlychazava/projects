﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsElasticService
    {
        /// <summary>
        /// Get product details by package Id
        /// </summary>
        /// <param name="id">Package Id</param>
        /// <returns>Product details</returns>
        Task<ProductDetailsDTO> GetByPackageId(int id);

        /// <summary>
        /// Add or update product details
        /// </summary>
        /// <param name="productDetails">Product details collection</param>
        /// <returns>Operation result</returns>
        Task<BulkOperationResult> AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails);

        /// <summary>
        /// Search product details by query string and filters
        /// </summary>
        /// <param name="query">Query string</param>
        /// <param name="filters">Filter options</param>
        /// <returns>Search results</returns>
        Task<SearchResultModel> Search(string query, FilterOptionsModel filters);

        /// <summary>
        /// Import product details from configured location
        /// </summary>
        /// <returns>Operation result</returns>
        Task<BulkOperationResult> ImportProductDetalils();

        /// <summary>
        /// Delete product details by package Id
        /// </summary>
        /// <param name="id">Package Id</param>
        /// <returns>True if deleted</returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Delete product details Index
        /// </summary>
        /// <returns>True if deleted</returns>
        Task<bool> DeleteIndex();
    }
}
