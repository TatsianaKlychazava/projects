using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;

namespace ApotekaShop.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            string elasticNodeUrl = ConfigurationManager.AppSettings["elasticNodeUrl"];
            string defaultIndex = ConfigurationManager.AppSettings["defaultIndex"];
            if (string.IsNullOrEmpty(elasticNodeUrl) || string.IsNullOrEmpty(defaultIndex)) throw new Exception("Elastic node Url or default index are not specified in web config");

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<ProductDetailsDataProvider>().As<IProductDetailsDataProvider>();
            builder.RegisterType<ProductDetailsElasticService>().As<IProductDetailsService>()
                .WithParameter(new TypedParameter(typeof(Uri), new Uri(elasticNodeUrl)))
                .WithParameter(new TypedParameter(typeof(string), defaultIndex));

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
