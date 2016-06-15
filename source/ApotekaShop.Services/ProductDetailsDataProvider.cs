using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Newtonsoft.Json;

namespace ApotekaShop.Services
{
    public class ProductDetailsDataProvider: IProductDetailsDataProvider
    {
        public List<ProductDetailsDTO> ImportProductDetalils()
        {
            var path = HostingEnvironment.MapPath(@"~/App_Data/productdetails.json");
            if (!File.Exists(path))
                throw new ApplicationException("Json file with sample data cannot be found.");

            var json = File.ReadAllText(path);
            var details = JsonConvert.DeserializeObject<List<ProductDetailsDTO>>(json);
            return details;
        }
    }
}
