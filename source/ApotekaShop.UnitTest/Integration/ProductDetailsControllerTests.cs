using System;
using ApotekaShop.Services.Models;
using ApotekaShop.UnitTest.Fixtures;
using ApotekaShop.Web.Models;
using Xunit;

namespace ApotekaShop.UnitTest.Integration
{
    public class ProductDetailsControllerTests : IClassFixture<ControllersTestFixture>, IDisposable
    {
        private readonly ControllersTestFixture _controllersTestFixture;

        private FilterOptionsViewModel filters = new FilterOptionsViewModel()
        {
            MaxPrice = 20,
            MinPrice = 10,
            Order = Order.Asc,
            OrderBy = "price",
            PageNumber = 2,
            Query = "migræne"
        };
        public ProductDetailsControllerTests(ControllersTestFixture controllersTestFixture)
        {
            _controllersTestFixture = controllersTestFixture;
        }

      

        public void Dispose()
        {
        }
    }
}
