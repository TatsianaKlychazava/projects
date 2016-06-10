﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.WebApi;
using Microsoft.Practices.Unity;
using Moq;
using Newtonsoft.Json;
using Unity.WebApi;
using Xunit;

namespace ApotekaShop.UnitTest.Fixtures
{
    public class ApiTestServerFixture: IDisposable
    {
        private const string _baseAddress = @"http://localhost";
        private const string JsonMediaTypeString = "application/json";

        private readonly HttpServer _server;
        private readonly HttpConfiguration _config;
        private readonly HttpClient _client;
        private readonly HttpCookieCollection _cookies = new HttpCookieCollection();
        private readonly Mock<HttpContextBase> _context = new Mock<HttpContextBase>();
        private readonly Mock<HttpResponseBase> _response = new Mock<HttpResponseBase>();
        private readonly Mock<IProductDetailsDataProvider> _dataprovider = new Mock<IProductDetailsDataProvider>();
        protected readonly IUnityContainer _container;

        public ApiTestServerFixture()
        {
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config);
          

            _context.SetupGet(c => c.Response).Returns(_response.Object);
            _response.SetupGet(c => c.Cookies).Returns(_cookies);
            
            _container = new UnityContainer();

            string elasticNodeUrl = ConfigurationManager.AppSettings["elasticNodeUrl"]; 
            string defaultIndex = ConfigurationManager.AppSettings["defaultIndex"];

            _dataprovider.Setup(x => x.ImportProductDetalils()).Returns(LoadTestData());
           
            _container.RegisterType<IProductDetailsService, ProductDetailsElasticService>(
                    new InjectionConstructor(_dataprovider.Object, new Uri(elasticNodeUrl), defaultIndex));

            _config.DependencyResolver = new UnityDependencyResolver(_container);
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            _server = new HttpServer(_config);

            _client = new HttpClient(_server);
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
            _server.Dispose();
        }
    }
}
