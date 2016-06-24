using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Web;
using Autofac;
using Autofac.Integration.WebApi;
using Moq;

namespace ApotekaShop.UnitTest.Fixtures
{
    public class ApiTestServerFixture : FixtureBase, IDisposable
    {
        private const string BaseAddress = @"http://localhost/api/ProductDetails/";
        private const string JsonMediaTypeString = "application/json";

        private readonly HttpServer _server;
        private readonly HttpCookieCollection _cookies = new HttpCookieCollection();
        private readonly Mock<HttpContextBase> _context = new Mock<HttpContextBase>();
        private readonly Mock<HttpResponseBase> _response = new Mock<HttpResponseBase>();
        private readonly HttpClient _client;

        public ApiTestServerFixture()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            
            _context.SetupGet(c => c.Response).Returns(_response.Object);
            _response.SetupGet(c => c.Cookies).Returns(_cookies);
         
            Builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Builder.RegisterWebApiFilterProvider(config);
            
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            _server = new HttpServer(config);
            _client = new HttpClient(_server);
        }

        public HttpRequestMessage CreateGetRequest(object address)
        {
            return CreateHttpRequest(address.ToString(), null, HttpMethod.Get);
        }
        public HttpRequestMessage CreateGetRequest(string address)
        {
            return CreateHttpRequest(address, null, HttpMethod.Get);
        }

        public HttpRequestMessage CreateDeletetRequest(string address)
        {
            return CreateHttpRequest(address, null, HttpMethod.Delete);
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
            request.RequestUri = new Uri(BaseAddress + address);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaTypeString));
            request.Method = method;

            if (method != HttpMethod.Delete || method != HttpMethod.Get)
            {
                request.Content = new ObjectContent<dynamic>(data, new JsonMediaTypeFormatter());
            }

            return request;
        }

        public void SendRequest(HttpRequestMessage request, Action<HttpResponseMessage> action)
        {
            using (HttpResponseMessage response = _client.SendAsync(request).Result)
            {
                action(response);
            }
        }
        
        public void Dispose()
        {
            var service = Container.Resolve<IProductDetailsElasticService>();
            service.DeleteIndex().Wait();

            _server.Dispose();
            _client.Dispose();
        }
    }
}
