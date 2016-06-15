using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApotekaShop.UnitTest.Extensions
{
    public static class HttpExtensions
    {
        public static T GetContent<T>(this HttpContent content)
        {
            Task<string> responseBody = content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody.Result);
        }
    }
}
