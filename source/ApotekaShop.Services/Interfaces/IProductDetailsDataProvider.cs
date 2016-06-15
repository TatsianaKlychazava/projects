using System.Collections.Generic;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsDataProvider
    {
        List<ProductDetailsDTO> ImportProductDetalils();
    }
}
