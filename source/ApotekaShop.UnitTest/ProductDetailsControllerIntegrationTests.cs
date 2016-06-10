﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using ApotekaShop.Services.Models;
using Xunit;

namespace ApotekaShop.UnitTest
{
    public class ProductDetailsControllerIntegrationTests: IClassFixture<ApiTestServerFixture>, IDisposable
    {
        private readonly ApiTestServerFixture _apiTestServerFixture;

        public ProductDetailsControllerIntegrationTests(ApiTestServerFixture apiTestServerFixture)
        {
            _apiTestServerFixture = apiTestServerFixture;

            var importRequest = _apiTestServerFixture.CreateGetRequest("/api/ProductDetails/ImportIndex");

            _apiTestServerFixture.SendRequest(importRequest, message =>
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new Exception("Can't import data");
                }
            } );
        }

        [Fact]
        public void Get_ProductDetailsById_ReturnsCorrectProductDetails()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest("/api/ProductDetails/104977");
            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });
        }

        [Fact]
        public void Post_ProductDetails_ProductDetailsAdded()
        {
            List<ProductDetailsDTO> details = new List<ProductDetailsDTO>()
            {
                new ProductDetailsDTO()
                {
                    PackageId = 123456
                }
            };

            var postRequest = _apiTestServerFixture.CreatePostRequest("/api/ProductDetails/", details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });
        }

        [Fact]
        public void Search_ProductDetails_ReturnsProductDetail()
        {
            List<ProductDetailsDTO> details = new List<ProductDetailsDTO>()
            {
                new ProductDetailsDTO()
                {
                    PackageId = 123456
                }
            };

            var postRequest = _apiTestServerFixture.CreateGetRequest("/api/ProductDetails/Search/?");

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });
        }

        public void Dispose()
        {
            var removeReques = _apiTestServerFixture.CreateGetRequest("/api/ProductDetails/RemoveIndex");
            _apiTestServerFixture.SendRequest(removeReques, message => { });
        }
    }
}
