using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IWebContext
    {
        void SetCountry(Country country);
        Country GetCountry();
    }
}