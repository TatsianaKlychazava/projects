using System.Collections.Generic;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsService
    {
        ProductDetailsDTO GetByPackageId(int id);
        void AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails);
        IEnumerable<ProductDetailsDTO> Search(string query, FilterOptionsModel filters);
        void ImportProductDetalils();
        void Delete(int id);
        void RemoveIndex();
    }
}
