using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly ITestOutputHelper _output;


        public ProductDetailsControllerTests(ApiTestServerFixture apiTestServerFixture, ITestOutputHelper output)
        {
            _apiTestServerFixture = apiTestServerFixture;
            _output = output;
        }

        #region Core functionality
        [Fact]
        public void Get_ProductDetailsById_WithCorrectId_ReturnsProductDetailsWithSelectedId()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(IdForSearch);

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
            var getRequest = _apiTestServerFixture.CreateGetRequest("0");

            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Get_ProductDetailsById_WithEmptyId_ReturnsBadReques()
        {
            var getRequest = _apiTestServerFixture.CreateGetRequest(string.Empty);

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

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Empty, details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
            });

            var id = 123456;
            var getRequest = _apiTestServerFixture.CreateGetRequest($"{id}");
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

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Empty, null);
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

            var postRequest = _apiTestServerFixture.CreatePostRequest(string.Empty, details);

            _apiTestServerFixture.SendRequest(postRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(IdForSearch);

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
            var id = 120661;
            var deleteRequest = _apiTestServerFixture.CreateDeletetRequest(id.ToString());

            _apiTestServerFixture.SendRequest(deleteRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
            });

            var getRequest = _apiTestServerFixture.CreateGetRequest(104977.ToString());
            _apiTestServerFixture.SendRequest(getRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        } 
        #endregion

        #region Search simple
        [Fact]
        public void Search_ProductDetails_WithCorrectEncodedQueryParameter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=aspirin%20kardio");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.Single(productDetailsList);
                Assert.Equal("aspirin kardio", productDetailsList.First().ProductNames.First().Name);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithCorrectQueryParameter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=aspirin kardio");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.Single(productDetailsList);
                Assert.Equal("aspirin kardio", productDetailsList.First().ProductNames.First().Name);
            });
        } 
        #endregion

        #region Search with filtering
        [Fact]
        public void Search_ProductDetails_WithMinPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&minPrice=4000");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.All(productDetailsList, x => Assert.True(x.NormalizedPrice >= 4000));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&maxPrice=5000");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.All(productDetailsList, x => Assert.True(x.NormalizedPrice <= 5000));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&minprice=4000&maxprice=5000");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.All(productDetailsList, x => Assert.True(x.NormalizedPrice >= 4000 && x.NormalizedPrice <= 5000));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithIncorrectMinAndMaxPriceFilter_ReturnsNotFound()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&minprice=5000&maxprice=4000");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithQueryAndMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=ultravist&minprice=100000&maxprice=40000000");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.All(productDetailsList, x =>
                {
                    Assert.True(x.NormalizedPrice >= 100000 && x.NormalizedPrice <= 40000000);
                    Assert.Equal("ultravist", productDetailsList.First().ProductNames.First().Name);
                });
            });
        }

        [Fact]
        public void Search_ProductDetails_WithInvalidQueryAndMinAndMaxPriceFilter_ReturnsNotFound()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=aspirin&minprice=100000&maxprice=40000000");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithEmptyQueryString_ReturnsBadRequest()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query= ");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithInvalidQueryString_ReturnsBadRequest()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=as");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, message.StatusCode);
            });
        } 
        #endregion

        #region Search with ordering
        [Fact]
        public void Search_ProductDetails_WithOrderByPriceAsc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&orderBy=price&order=asc");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                var expectedList = productDetailsList.OrderBy(x => x.NormalizedPrice);

                Assert.Equal(expectedList, productDetailsList);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderByPriceDesc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&orderBy=price&order=desc");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                var expectedList = productDetailsList.OrderByDescending(x => x.NormalizedPrice);

                Assert.Equal(expectedList, productDetailsList);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderBySizeAsc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&orderBy=size&order=asc");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                var expectedList = productDetailsList.OrderBy(x => x.NormalizedPackageSize);

                Assert.Equal(expectedList, productDetailsList);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderBySizeDesc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&orderBy=size&order=desc");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                var expectedList = productDetailsList.OrderByDescending(x => x.NormalizedPackageSize);

                Assert.Equal(expectedList, productDetailsList);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithCorrectLCID_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&lcid=1030");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.NotEmpty(productDetailsList);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithEmptyLCID_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);
                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.NotEmpty(productDetailsList);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithIncorrectLCID_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&lcid=2");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode); 
            });
        }
        #endregion

        #region Search with paging
        [Fact]
        public void Search_ProductDetails_WithPaging_ReturnsCorrectPage()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=aspirin&pageSize=1&pageFrom=0");

            IEnumerable<ProductDetailsDTO> firstPageItems = new List<ProductDetailsDTO>();

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                firstPageItems = message.Content.GetContent<SearchResultModel>().Results;
            });

            searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=aspirin&pageSize=1&pageFrom=1");

            IEnumerable<ProductDetailsDTO> secondPageItems = new List<ProductDetailsDTO>(); ;
            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                secondPageItems = message.Content.GetContent<SearchResultModel>().Results;
            });

            Assert.NotEqual(firstPageItems, secondPageItems);

            searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=aspirin&pageSize=1&pageFrom=2");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithPaging_ReturnsItemsCount()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&pageSize=10");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.Equal(productDetailsList.Count(), 10);
            });

            searchRequest = _apiTestServerFixture.CreateGetRequest("Search/?query=migræne&pageSize=20");

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.Equal(HttpStatusCode.OK, message.StatusCode);

                IEnumerable<ProductDetailsDTO> productDetailsList = message.Content.GetContent<SearchResultModel>().Results;

                Assert.Equal(productDetailsList.Count(), 20);
            });
        } 
        #endregion

        public void Dispose()
        {
        }
    }
}
