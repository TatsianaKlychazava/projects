using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using System;
using System.Configuration;

namespace ApotekaShop.Services
{
    public class ConfigurationSettingsProvider: IConfigurationSettingsProvider
    {
        private ConfigurationSettingsModel _current;

        public ConfigurationSettingsModel GetConfiguration()
        {
            if (_current == null)
            {
                LoadConfiguration();
            }

            return _current;
        }

        public void SetConfiguration(ConfigurationSettingsModel config)
        {
            _current = config;
        }

        private void LoadConfiguration()
        {
            _current = new ConfigurationSettingsModel
            {
                MinQueryLength = int.Parse(ConfigurationManager.AppSettings["minQueryLength"]),
                ElasticNodeUrl = new Uri(ConfigurationManager.AppSettings["elasticNodeUrl"]),
                DefaultIndex = ConfigurationManager.AppSettings["defaultIndex"],
                DefaultPageSize = int.Parse(ConfigurationManager.AppSettings["defaultPageSize"])
            };
        }
    }
}
