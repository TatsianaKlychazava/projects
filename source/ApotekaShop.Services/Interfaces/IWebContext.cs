using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    /// <summary>
    /// Web context wrapper
    /// </summary>
    public interface IWebContext
    {
        /// <summary>
        /// Check if connection secured
        /// </summary>
        bool IsConnectionSecured { get; }

        /// <summary>
        /// Returns protocol
        /// </summary>
        string Protocol { get; }
        
        /// <summary>
        /// Returns http host
        /// </summary>
        string HttpHost { get; }

        /// <summary>
        /// Set country
        /// </summary>
        /// <param name="country">Country</param>
        void SetCountry(Country country);

        /// <summary>
        /// Get current country
        /// </summary>
        /// <returns>Country</returns>
        Country GetCountry();
    }
}