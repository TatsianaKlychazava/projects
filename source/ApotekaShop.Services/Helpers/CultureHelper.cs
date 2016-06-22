using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
