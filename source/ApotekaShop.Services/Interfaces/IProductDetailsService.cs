using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsService
    {
        ProductDetailsDTO GetByPackageId(int id);
        void AddOrUpdate(IEnumerable<ProductDetailsDTO> productDetails);
        IEnumerable<ProductDetailsDTO> Search(string query, FilterOptionsModel filters);
        void ImportProductDetalils();
    }
}
