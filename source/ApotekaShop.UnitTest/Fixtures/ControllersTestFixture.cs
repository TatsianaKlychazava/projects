using System;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using Autofac;

namespace ApotekaShop.UnitTest.Fixtures
{
    public class ControllersTestFixture : FixtureBase, IDisposable
    {

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public void SetCountry(Country country)
        {
            WebContext.Setup(x => x.GetCountry()).Returns(country);
        }

        public void Dispose()
        {
            var service = Container.Resolve<IProductDetailsElasticService>();
            service.DeleteIndex().Wait();
        }
    }
}
