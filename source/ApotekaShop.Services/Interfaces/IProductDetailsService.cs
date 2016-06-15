﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsService
    {
        Task<ProductDetailsDTO> GetByPackageId(int id);
        Task<ElasticBulkOperationResult> AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails);
        Task<IEnumerable<ProductDetailsDTO>> Search(string query, FilterOptionsModel filters);
        Task<ElasticBulkOperationResult> ImportProductDetalils();
        Task Delete(int id);
        Task DeleteIndex();
    }
}
