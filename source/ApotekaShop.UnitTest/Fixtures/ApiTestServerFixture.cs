using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.UnitTest.Extensions;
using ApotekaShop.WebApi;
using ApotekaShop.WebApi.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ApotekaShop.UnitTest.Fixtures
{
    public class ApiTestServerFixture : IDisposable
    {
        private const string _baseAddress = @"http://localhost/api/ProductDetails/";
        private const string JsonMediaTypeString = "application/json";

        private readonly HttpServer _server;
        private readonly HttpConfiguration _config;
        private readonly HttpCookieCollection _cookies = new HttpCookieCollection();
        private readonly Mock<HttpContextBase> _context = new Mock<HttpContextBase>();
        private readonly Mock<HttpResponseBase> _response = new Mock<HttpResponseBase>();
        private readonly Mock<IProductDetailsDataProvider> _dataprovider = new Mock<IProductDetailsDataProvider>();
        private readonly HttpClient _client;
        private readonly IContainer _container;

        public ApiTestServerFixture()
        {
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config);


            _context.SetupGet(c => c.Response).Returns(_response.Object);
            _response.SetupGet(c => c.Cookies).Returns(_cookies);
            _dataprovider.Setup(x => x.ImportProductDetalils()).Returns(LoadTestData());

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(_config);

            builder.RegisterType<ConfigurationSettingsProvider>().As<IConfigurationSettingsProvider>();
            builder.RegisterInstance(_dataprovider.Object).As<IProductDetailsDataProvider>();
            builder.RegisterType<ProductDetailsElasticService>().As<IProductDetailsService>()
                .WithParameter(new TypedParameter(typeof(IProductDetailsDataProvider), _dataprovider.Object));

            builder.RegisterType<ProductDetailsController>().InstancePerRequest();

            _container = builder.Build();

            InitIndex();


            _config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            _server = new HttpServer(_config);
            _client = new HttpClient(_server);
        }

        public void InitIndex()
        {
            var service = _container.Resolve<IProductDetailsService>();
            var result = service.ImportProductDetalils().Result;
            if (result.HasErrors)
                throw new TestClassException("Cannot create initial index.");
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
            request.RequestUri = new Uri(_baseAddress + address);
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

        private List<ProductDetailsDTO> LoadTestData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = ConfigurationManager.AppSettings["dataResourceName"];
            string json;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            var details = JsonConvert.DeserializeObject<List<ProductDetailsDTO>>(json);

            return details;
        }

        public void Dispose()
        {
            var service = _container.Resolve<IProductDetailsService>();
            service.DeleteIndex().Wait();

            _server.Dispose();
            _client.Dispose();
        }
    }
}
