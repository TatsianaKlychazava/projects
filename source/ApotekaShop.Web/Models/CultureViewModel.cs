using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace ApotekaShop.Web.Models
{
    public class CultureViewModel
    {
        public IEnumerable<CultureInfo> Cultures => CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Except(CultureInfo.GetCultures(CultureTypes.SpecificCultures));
        
        public CultureInfo SelectedCulture { get; set; }
    }
}