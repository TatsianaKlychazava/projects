using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IWebContext
    {
        bool IsConnectionSecured { get; }
        string Protocol { get; }
        string HttpHost { get; }
        void SetCountry(Country country);
        Country GetCountry();
    }
}