using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IProductDetailsDataProvider
    {
        List<ProductDetailsDTO> ImportProductDetalils();
    }
}
