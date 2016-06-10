using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.WebApi;
using Microsoft.Practices.Unity;
using Moq;
using Unity.WebApi;
using Xunit;

namespace ApotekaShop.UnitTest
{
    public class ApiTestFixtureBase
    {
        private const string _baseAddress = @"http://localhost";
        private readonly HttpServer _server;
        private readonly HttpConfiguration _config;
        private readonly HttpClient _client;
        private readonly HttpCookieCollection _cookies = new HttpCookieCollection();
        private readonly Mock<HttpContextBase> _context = new Mock<HttpContextBase>();
        private readonly Mock<HttpResponseBase> _response = new Mock<HttpResponseBase>();

        protected readonly IUnityContainer _container;

        public ApiTestFixtureBase()
        {
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config);
          

            _context.SetupGet(c => c.Response).Returns(_response.Object);
            _response.SetupGet(c => c.Cookies).Returns(_cookies);
            
            _container = new UnityContainer();

            string elasticNodeUrl = "http://localhost:9200";
            string defaultIndex = "testindex";

            _container.RegisterType<IProductDetailsService, ProductDetailsElasticService>(
                    new InjectionConstructor(new Uri(elasticNodeUrl), defaultIndex));
                

            _config.DependencyResolver = new UnityDependencyResolver(_container);
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            _server = new HttpServer(_config);

            _client = new HttpClient(_server);
        }

        public HttpRequestMessage CreateGetRequest(string address)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseAddress + address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = HttpMethod.Get;

            return request;
        }

        public HttpRequestMessage CreatePutRequest(string address, object data)
        {
            return CreateHttpRequest(address, data, HttpMethod.Put);
        }

        public HttpRequestMessage CreatePostRequest(string address, object data)
        {
            return CreateHttpRequest(address, data, HttpMethod.Post);
        }

        public HttpRequestMessage CreateHttpRequest(string address, object data, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(_baseAddress + address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;
            request.Content = new ObjectContent<dynamic>(data, new JsonMediaTypeFormatter());

            return request;
        }

        public void SendRequest(HttpRequestMessage request, Action<HttpResponseMessage> sucessAction)
        {
            using (HttpResponseMessage response = _client.SendAsync(request).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    sucessAction(response);
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Assert.True(response.IsSuccessStatusCode, result);
                }
            }
        }
    }
}
