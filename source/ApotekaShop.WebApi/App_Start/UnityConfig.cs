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
            string defaultIndex = ConfigurationManager.AppSettings["defaultIndex"];
            if (string.IsNullOrEmpty(elasticNodeUrl) || string.IsNullOrEmpty(defaultIndex)) throw new Exception("Elastic node Url or default index are not specified in web config");

            container.RegisterType<IProductDetailsDataProvider, ProductDetailsDataProvider>();
            container.RegisterType<IProductDetailsService, ProductDetailsElasticService>(new InjectionConstructor(typeof(IProductDetailsDataProvider),new Uri(elasticNodeUrl),defaultIndex));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}