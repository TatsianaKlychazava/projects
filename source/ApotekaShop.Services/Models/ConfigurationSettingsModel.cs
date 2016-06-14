using System;

namespace ApotekaShop.Services.Models
{
    public class ConfigurationSettingsModel
    {
        public Uri ElasticNodeUrl { get; set; }
        public string DefaultIndex { get; set; }
        public int MinQueryLength { get; set; }
        public int DefaultPageSize { get; set; }
    }
}
