using System;
using System.Collections.Generic;

namespace ApotekaShop.Services.Models
{
    public class ConfigurationSettingsModel
    {
        public Uri ElasticNodeUrl { get; set; }
        public string DefaultIndex { get; set; }
        public int MinQueryLength { get; set; }
        public int DefaultPageSize { get; set; }

        public Dictionary<string, string> FilterOptions = new Dictionary<string, string>()
        {
            { "price", nameof(ProductDetailsDTO.NormalizedPrice) },
            { "size", nameof(ProductDetailsDTO.NormalizedPackageSize)}
        };

    }
}
