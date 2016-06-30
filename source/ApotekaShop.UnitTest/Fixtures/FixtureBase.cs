using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.Web.Controllers;
using Autofac;
using Moq;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace ApotekaShop.UnitTest.Fixtures
{
    public class FixtureBase
    {
        private readonly Mock<IProductDetailsDataProvider> _dataprovider = new Mock<IProductDetailsDataProvider>();
        protected readonly Mock<IWebContext> WebContext = new Mock<IWebContext>();
        protected IContainer Container;
        protected ContainerBuilder Builder { get; set; }

        public FixtureBase()
        {
            Builder = new ContainerBuilder();

            _dataprovider.Setup(x => x.ImportProductDetails()).Returns(LoadTestData());
            Builder.RegisterInstance(WebContext.Object).As<IWebContext>();
            Builder.RegisterType<ProductDetailsController>();
            Builder.RegisterType<ConfigurationSettingsProvider>().As<IConfigurationSettingsProvider>();
            Builder.RegisterInstance(_dataprovider.Object).As<IProductDetailsDataProvider>();
            Builder.RegisterType<ProductDetailsElasticService>().As<IProductDetailsElasticService>()
                .WithParameter(new TypedParameter(typeof(IProductDetailsDataProvider), _dataprovider.Object));
            Builder.RegisterType<ProductDetailsService>().As<IProductDetailsService>();

            Builder.RegisterType<ProductDetailsApiController>().InstancePerRequest();

            Container = Builder.Build();

            InitIndex();
        }

        public void InitIndex()
        {
            var service = Container.Resolve<IProductDetailsElasticService>();
            var result = service.ImportProductDetalils().Result;
            if (result.HasErrors)
                throw new TestClassException("Cannot create initial index.");
        }

        protected List<ProductDetailsDTO> LoadTestData()
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
    }
}
