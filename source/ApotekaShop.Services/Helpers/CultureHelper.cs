using System.Globalization;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Helpers
{
    public static class CultureHelper
    {
        public static CultureInfo ToCulture(this Country country)
        {
            switch (country)
            {
                case Country.SE:
                    return new CultureInfo("sv-SE");
                case Country.DK:
                default:
                    return new CultureInfo("da-DK");
            }
        }
    }
}
