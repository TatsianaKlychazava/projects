using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;
using ApotekaShop.UnitTest.Fixtures;
using Xunit;

namespace ApotekaShop.UnitTest.Integration
{
    public class ProductDetailsControllerTests: IClassFixture<ApiTestServerFixture>, IDisposable
    {
        private readonly ApiTestServerFixture _apiTestServerFixture;
        private const int IdForSearch = 114315;
        private const string BaseUrl = "/api/ProductDetails/{0}";

        public ProductDetailsControllerTests(ApiTestServerFixture apiTestServerFixture)
        {
            _apiTestServerFixture = apiTestServerFixture;

            var importRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "ImportIndex"));

            _apiTestServerFixture.SendRequest(importRequest, message =>
            {
                if (!message.IsSuccessStatusCode)
                {
                    throw new Exception("Can't import data");
                }
            } );
        }

        [Fact]
        public void Get_ProductDetailsById_WithCorrectId_ReturnsProductDetailsWithSelectedId()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, IdForSearch));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);

                ProductDetailsDTO productDetails = _apiTestServerFixture.GetContent<ProductDetailsDTO>(message.Content);

                Assert.Equal(IdForSearch, productDetails.PackageId);
            });
        }

        [Fact]
        public void Get_ProductDetailsById_WithIncorrectId_ReturnsNotFoundResult()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, 123));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public void Get_ProductDetailsById_WithEmptyId_ReturnsBadReques()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl,""));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.MethodNotAllowed);
            });
        }


        [Fact]
        public void Post_ProductDetails_WithNewData_ProductDetailsAdded()
        {
            List<ProductDetailsDTO> details = new List<ProductDetailsDTO>()
            {
                new ProductDetailsDTO()
                {
                    PackageId = 123456
                }
            };

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Format(BaseUrl, ""), details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, 123456));
            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });
        }

        [Fact]
        public void Post_ProductDetails_WithEmptyBody_ReturnsBadReques()
        {
            
            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Format(BaseUrl, ""), null);
            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.BadRequest);
            });
        }

        [Fact]
        public void Post_ProductDetails_WithExistingData_ProductDetailsUpdated()
        {
            List<ProductDetailsDTO> details = new List<ProductDetailsDTO>()
            {
                new ProductDetailsDTO()
                {
                    PackageId = IdForSearch,
                    ProductId = 100
                }
            };

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Format(BaseUrl, ""), details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, IdForSearch));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);

                ProductDetailsDTO productDetails = _apiTestServerFixture.GetContent<ProductDetailsDTO>(message.Content);

                Assert.Equal(IdForSearch, productDetails.PackageId);
                Assert.Equal(100, productDetails.ProductId);
            });
        }

        [Fact]
        public void Delete_ProductDetails_ReturnsDone()
        {
            var deleteRequest = _apiTestServerFixture.CreateDeletetRequest(string.Format(BaseUrl, 104977));

            _apiTestServerFixture.SendRequest(deleteRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, 104977));
            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public void Search_ProductDetails_ReturnsProductDetail()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("/api/ProductDetails/Search/?query=aspirin");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
            });
        }
    
        public void Dispose()
        {
            var removeReques = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "RemoveIndex"));
            _apiTestServerFixture.SendRequest(removeReques, message => { });
        }
    }
}
