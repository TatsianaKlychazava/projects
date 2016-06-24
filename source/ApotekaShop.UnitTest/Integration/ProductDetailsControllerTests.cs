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

        private readonly FilterOptionsViewModel _filters = new FilterOptionsViewModel()
        {
            MaxPrice = 200,
            MinPrice = 100,
            Order = Order.Asc,
            OrderBy = "price",
            PageNumber = 2,
            Query = "migræne"
        };

        public ProductDetailsControllerTests(ControllersTestFixture controllersTestFixture)
        {
            _controllersTestFixture = controllersTestFixture;
        }

        [Fact]
        public void On_Search_WithCorrentFiltersAndPagingAndCountry_ReturnsProductDetails()
        {
            ProductDetailsController controller = _controllersTestFixture.Resolve<ProductDetailsController>();

            _controllersTestFixture.SetCountry(Country.DK);

            Task<ActionResult> result = controller.Search(_filters);

            ProductDetailsViewModel resultModel = ((ViewResult) result.Result).Model as ProductDetailsViewModel;

            Assert.NotNull(resultModel);

            Assert.Equal(43, resultModel.Total);
            
            Assert.All(resultModel.Products, x => Assert.True(x.NormalizedPrice >=  _filters.MinPrice*100 && x.NormalizedPrice <= _filters.MaxPrice*100));

            Assert.All(resultModel.Products, x =>
            {
                bool contains = false;
                foreach (var indication in x.Indications)
                {
                    if (indication.Name == _filters.Query)
                    {
                        contains = true;
                    }
                   
                }
                Assert.True(contains);
            });
        }

        [Fact]
        public void On_Search_WithCorrentFiltersAndPagingAndIncorrectCountry_ReturnsProductDetails()
        {
            ProductDetailsController controller = _controllersTestFixture.Resolve<ProductDetailsController>();

            _controllersTestFixture.SetCountry(Country.SE);

            Task<ActionResult> result = controller.Search(_filters);

            ProductDetailsViewModel resultModel = ((ViewResult)result.Result).Model as ProductDetailsViewModel;

            Assert.NotNull(resultModel);

            Assert.Equal(0, resultModel.Total);
        }

        public void Dispose()
        {
        }
    }
}
