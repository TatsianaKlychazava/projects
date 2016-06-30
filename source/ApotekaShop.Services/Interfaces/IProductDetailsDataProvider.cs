using System.Collections.Generic;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    /// <summary>
    /// Provider for product details dtos import
    /// </summary>
    public interface IProductDetailsDataProvider
    {
        /// <summary>
        /// Import product details
        /// </summary>
        /// <returns>Product Details DTO collection</returns>
        List<ProductDetailsDTO> ImportProductDetails();
    }
}
