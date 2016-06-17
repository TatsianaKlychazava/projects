using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace ApotekaShop.Web
{
    public static class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<ConfigurationSettingsProvider>().As<IConfigurationSettingsProvider>().InstancePerRequest();
            builder.RegisterType<ProductDetailsDataProvider>().As<IProductDetailsDataProvider>();
            builder.RegisterType<ProductDetailsElasticService>().As<IProductDetailsService>();


            var container = builder.Build();
            
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = webApiResolver;

            // Set the dependency resolver for MVC.
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ConfigurationSettingsProvider>().As<IConfigurationSettingsProvider>().InstancePerRequest();
            builder.RegisterType<ProductDetailsDataProvider>().As<IProductDetailsDataProvider>();
            builder.RegisterType<ProductDetailsElasticService>().As<IProductDetailsService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Autofac.Integration.WebApi.AutofacWebApiDependencyResolver(container);
        }
    }
}