using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using ApotekaShop.Services;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;
using ApotekaShop.UnitTest.Fixtures;
using ApotekaShop.Web.Controllers;
using ApotekaShop.Web.Models;
using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

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
