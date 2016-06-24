using System;

namespace ApotekaShop.Web.Models
{
    public class SuggestionViewModel
    {
        public SuggestionViewModel(string suggestion, string query)
        {
            Value = suggestion;
            var indexOf = suggestion.IndexOf(query, StringComparison.Ordinal);

            Name = suggestion.Insert(indexOf, "<b>").Insert(indexOf + 3 + query.Length, "</b>");
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}