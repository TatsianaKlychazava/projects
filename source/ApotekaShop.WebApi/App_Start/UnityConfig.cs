using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using System.Web.Http;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using Unity.WebApi;

namespace ApotekaShop.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            string elasticNodeUrl = ConfigurationManager.AppSettings["elasticNodeUrl"];

            if (string.IsNullOrEmpty(elasticNodeUrl)) throw new Exception("Elastic node Url is not specified in web config");

            container.RegisterType<IProductDetailsService, ProductDetailsElasticService>(new InjectionConstructor(new Uri(elasticNodeUrl)));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}