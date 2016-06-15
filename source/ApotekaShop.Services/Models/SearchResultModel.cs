using System.Collections.Generic;

namespace ApotekaShop.Services.Models
{
    public class SearchResultModel
    {
        public IEnumerable<ProductDetailsDTO> Results { get; set; }
        public int ExecutionTime { get; set; }
        public long TotalResults { get; set; }
    }
}
