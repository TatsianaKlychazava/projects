using System.Collections.Generic;
using System.IO;
using System.Web;
using ApotekaShop.Services.Models;
using Newtonsoft.Json;

namespace ApotekaShop.Services
{
    public class ProductDetailsDataProvider
    {
        public static List<ProductDetailsDTO> ImportProductDetalils()
        {
            using (StreamReader r = new StreamReader(Path.Combine(Directory.GetParent(HttpContext.Current.Server.MapPath("~")).Parent.Parent.FullName, "data\\productdetails.json")))
            {
                string json = r.ReadToEnd();
                var details = JsonConvert.DeserializeObject<List<ProductDetailsDTO>>(json);

                return details;
            }
        }
    }
}
