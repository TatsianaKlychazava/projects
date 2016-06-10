using System.Collections.Generic;

namespace ApotekaShop.Services.Models
{
    public class ProductDetailsDTO
    {
        public long ProductId { get; set; }
        public long PackageId { get; set; }
        public decimal NormalizedPackageSize { get; set; }
        public string NormalizedUnit { get; set; }
        public string DisplayText { get; set; }
        public int LCID { get; set; }
        public decimal NormalizedPrice { get; set; }
        public string ProductDescription { get; set; }

        public IEnumerable<ProductName> ProductNames { get; set; }

        public IEnumerable<ATCCode> ATCCodes { get; set; }
        public IEnumerable<Indication> Indications { get; set; }
        public IEnumerable<Treament> Treaments { get; set; }
        public IEnumerable<Substance> Substances { get; set; }

        public IEnumerable<ProductDetailsDTO> Substitutions { get; set; }
    }

    public class ProductName
    {
        public string Name { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
    }

    public class Indication
    {
        public string Name { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
    }

    public class Treament
    {
        public string Name { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
    }

    public class ATCCode
    {
        public string Name { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
    }

    public class Substance
    {
        public string Name { get; set; }
        public IEnumerable<string> Synonyms { get; set; }
    }
}
