using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ApotekaShop.Services.Models;
using ApotekaShop.UnitTest.Extensions;
using ApotekaShop.UnitTest.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace ApotekaShop.UnitTest.Integration
{
    public class ProductDetailsControllerTests : IClassFixture<ApiTestServerFixture>, IDisposable
    {
        private readonly ApiTestServerFixture _apiTestServerFixture;
        private const int IdForSearch = 114315;
        private const string BaseUrl = "/api/ProductDetails/{0}";
        private readonly ITestOutputHelper _output;


        public ProductDetailsControllerTests(ApiTestServerFixture apiTestServerFixture, ITestOutputHelper output)
        {
            _apiTestServerFixture = apiTestServerFixture;
            _output = output;

            //_output.WriteLine("Prepare Index.");
            //var importRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "ImportIndex"));

            //_apiTestServerFixture.SendRequest(importRequest, message =>
            //{
            //    if (!message.IsSuccessStatusCode)
            //    {
            //        throw new Exception("Can't import data");
            //    }
            //    var result = message.Content.GetContent<ElasticBulkOperationResult>();
            //    _output.WriteLine(result.ToJson());
            //});
        }

        [Fact]
        public void Get_ProductDetailsById_WithCorrectId_ReturnsProductDetailsWithSelectedId()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, IdForSearch));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                ProductDetailsDTO productDetails = message.Content.GetContent<ProductDetailsDTO>();

                Assert.Equal(IdForSearch, productDetails.PackageId);
            });
        }

        [Fact]
        public void Get_ProductDetailsById_WithIncorrectId_ReturnsNotFoundResult()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, 123));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Get_ProductDetailsById_WithEmptyId_ReturnsBadReques()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, string.Empty));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.MethodNotAllowed, message.StatusCode);
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

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Format(BaseUrl, string.Empty), details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
            });

            var id = 123456;
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, id));
            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                ProductDetailsDTO productDetails = message.Content.GetContent<ProductDetailsDTO>();

                Assert.Equal(id, productDetails.PackageId);
            });
        }

        [Fact]
        public void Post_ProductDetails_WithEmptyBody_ReturnsBadReques()
        {

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Format(BaseUrl, string.Empty), null);
            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, message.StatusCode);
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

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Format(BaseUrl, string.Empty), details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, IdForSearch));

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                ProductDetailsDTO productDetails = message.Content.GetContent<ProductDetailsDTO>();

                Assert.Equal(IdForSearch, productDetails.PackageId);
                Assert.Equal(100, productDetails.ProductId);
            });
        }

        [Fact]
        public void Delete_ProductDetails_ReturnsDone()
        {
            var id = 104977;
            var deleteRequest = _apiTestServerFixture.CreateDeletetRequest(string.Format(BaseUrl, id));

            _apiTestServerFixture.SendRequest(deleteRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, 104977));
            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithCorrectEncodedQueryParameter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin%20kardio"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                List<ProductDetailsDTO> productDetailsList = message.Content.GetContent<List<ProductDetailsDTO>>();

                Assert.Single(productDetailsList);
                Assert.Equal("aspirin kardio", productDetailsList.First().ProductNames.First().Name);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithCorrectQueryParameter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin kardio"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                List<ProductDetailsDTO> productDetailsList = message.Content.GetContent<List<ProductDetailsDTO>>();

                Assert.Single(productDetailsList);
                Assert.Equal("aspirin kardio", productDetailsList.First().ProductNames.First().Name);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMinPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&minPrice=4000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                List<ProductDetailsDTO> productDetailsList = message.Content.GetContent<List<ProductDetailsDTO>>();

                Assert.All(productDetailsList, x => Assert.True(x.NormalizedPrice >= 4000));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&maxPrice=5000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                List<ProductDetailsDTO> productDetailsList = message.Content.GetContent<List<ProductDetailsDTO>>();

                Assert.All(productDetailsList, x => Assert.True(x.NormalizedPrice <= 5000));

            });
        }

        [Fact]
        public void Search_ProductDetails_WithMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&minprice=4000&maxprice=5000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                List<ProductDetailsDTO> productDetailsList = message.Content.GetContent<List<ProductDetailsDTO>>();

                Assert.All(productDetailsList, x => Assert.True(x.NormalizedPrice >= 4000 && x.NormalizedPrice <= 5000));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithIncorrectMinAndMaxPriceFilter_ReturnsNotFound()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&minprice=5000&maxprice=4000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithQueryAndMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=ultravist&minprice=100000&maxprice=40000000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                List<ProductDetailsDTO> productDetailsList = message.Content.GetContent<List<ProductDetailsDTO>>();

                Assert.All(productDetailsList, x =>
                {
                    Assert.True(x.NormalizedPrice >= 100000 && x.NormalizedPrice <= 40000000);
                    Assert.Equal("ultravist", productDetailsList.First().ProductNames.First().Name);
                });
            });
        }

        [Fact]
        public void Search_ProductDetails_WithInvalidQueryAndMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin&minprice=100000&maxprice=40000000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithEmptyQueryString_ReturnsBadRequest()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query= "));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithInvalidQueryString_ReturnsBadRequest()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=as"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, message.StatusCode);
            });
        }

        public void Dispose()
        {
            //var removeReques = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "RemoveIndex"));
            //_apiTestServerFixture.SendRequest(removeReques, message => { });
        }
    }
}
