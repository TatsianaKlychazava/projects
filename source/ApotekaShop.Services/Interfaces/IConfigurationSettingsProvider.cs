using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    /// <summary>
    /// Store configuration settingsS
    /// </summary>
    public interface IConfigurationSettingsProvider
    {
        /// <summary>
        /// Set configuration settings 
        /// </summary>
        /// <param name="config">ConfigurationSettingsModel</param>
        void SetConfiguration(ConfigurationSettingsModel config);
        
        /// <summary>
        /// Get configuration settings model
        /// </summary>
        /// <returns>ConfigurationSettingsModel</returns>
        ConfigurationSettingsModel GetConfiguration();
    }
}
