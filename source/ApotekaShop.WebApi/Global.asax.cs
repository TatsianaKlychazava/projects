using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using System.Net;

namespace ApotekaShop.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;
                     
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<ConfigurationSettingsProvider>().As<IConfigurationSettingsProvider>();
            builder.RegisterType<ProductDetailsDataProvider>().As<IProductDetailsDataProvider>();
            builder.RegisterType<ProductDetailsElasticService>().As<IProductDetailsService>();


            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
