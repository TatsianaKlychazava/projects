using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IConfigurationSettingsProvider
    {
        void SetConfiguration(ConfigurationSettingsModel config);
        ConfigurationSettingsModel GetConfiguration();
    }
}
