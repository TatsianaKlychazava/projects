using System.Collections.Generic;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsService
    {
        Task<ProductDetailsDTO> GetByPackageId(int id);
        Task<BulkOperationResult> AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails);
        Task<SearchResultModel> Search(string query, FilterOptionsModel filters);
        Task<BulkOperationResult> ImportProductDetalils();
        Task<bool> Delete(int id);
        Task<bool> DeleteIndex();
        Task<IEnumerable<string>> GetSuggestions(string query);
    }
}
