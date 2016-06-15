using Newtonsoft.Json;

namespace ApotekaShop.UnitTest.Extensions
{
    public static class StringExtentions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
