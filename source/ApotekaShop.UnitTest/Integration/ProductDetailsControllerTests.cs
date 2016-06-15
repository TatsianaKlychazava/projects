using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ApotekaShop.Services.Models;
using ApotekaShop.UnitTest.Fixtures;
using Xunit;
using System.Threading;

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
                ProductDetailsDTO productDetails = _apiTestServerFixture.GetContent<ProductDetailsDTO>(message.Content);

                Assert.Equal(123456, productDetails.PackageId);
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
        public void Search_ProductDetails_WithCorrectEncodedQueryParameter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin%20kardio"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);
                
                Assert.True(productDetailsList.Count() == 1);
                Assert.True(productDetailsList.First().ProductNames.First().Name == "aspirin kardio");
            });
        }

        [Fact]
        public void Search_ProductDetails_WithCorrectQueryParameter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin kardio"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);
                
                Assert.True(productDetailsList.Count() == 1);
                Assert.True(productDetailsList.First().ProductNames.First().Name == "aspirin kardio");
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMinPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&minPrice=4000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                foreach (var productDetails in productDetailsList)
                {
                    Assert.True(productDetails.NormalizedPrice >= 4000);
                }
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&maxPrice=5000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);
                
                foreach (var productDetails in productDetailsList)
                {
                    Assert.True(productDetails.NormalizedPrice <= 5000);
                }
            });
        }

        [Fact]
        public void Search_ProductDetails_WithMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&minprice=4000&maxprice=5000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                foreach (var productDetails in productDetailsList)
                {
                    Assert.True(productDetails.NormalizedPrice >= 4000 && productDetails.NormalizedPrice <= 5000);
                }
            });
        }

        [Fact]
        public void Search_ProductDetails_WithIncorrectMinAndMaxPriceFilter_ReturnsNotFound()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&minprice=5000&maxprice=4000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
               Assert.True(message.StatusCode == HttpStatusCode.NotFound);        
            });
        }

        [Fact]
        public void Search_ProductDetails_WithQueryAndMinAndMaxPriceFilter_ReturnsProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=ultravist&minprice=100000&maxprice=40000000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);

                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                foreach(var productDetails in productDetailsList)
                {
                    Assert.True(productDetails.NormalizedPrice >= 100000 && productDetails.NormalizedPrice <= 40000000);
                    Assert.True(productDetailsList.First().ProductNames.First().Name == "ultravist");
                }

            });
        }

        [Fact]
        public void Search_ProductDetails_WithInvalidQueryAndMinAndMaxPriceFilter_ReturnsNotFound()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin&minprice=100000&maxprice=40000000"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithEmptyQueryString_ReturnsBadRequest()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query= "));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.BadRequest);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithInvalidQueryString_ReturnsBadRequest()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=as"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.BadRequest);
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderByPriceAsc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&orderBy=price&order=asc"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                var exceptedList = productDetailsList.OrderBy(x => x.NormalizedPrice);

                Assert.True(exceptedList.SequenceEqual(productDetailsList));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderByPriceDesc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&orderBy=price&order=desc"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                var exceptedList = productDetailsList.OrderByDescending(x => x.NormalizedPrice);

                Assert.True(exceptedList.SequenceEqual(productDetailsList));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderBySizeAsc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&orderBy=size&order=asc"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                var exceptedList = productDetailsList.OrderBy(x => x.NormalizedPackageSize);

                Assert.True(exceptedList.SequenceEqual(productDetailsList));
            });
        }

        [Fact]
        public void Search_ProductDetails_WithOrderBySizeDesc_ReturnsOrderedProductDetails()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=migræne&orderBy=size&order=desc"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                List<ProductDetailsDTO> productDetailsList = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);

                var exceptedList = productDetailsList.OrderByDescending(x => x.NormalizedPackageSize);

                Assert.True(exceptedList.SequenceEqual(productDetailsList));
            });
        }
       
        [Fact]
        public void Search_ProductDetails_WithPaging_ReturnsCorrectPage()
        {
            var searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin&pageSize=1&pageFrom=0"));

            List<ProductDetailsDTO> firstPageItems = new List<ProductDetailsDTO>();

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                firstPageItems = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);
            });

            searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin&pageSize=1&pageFrom=1"));

            List<ProductDetailsDTO> secondPageItems = new List<ProductDetailsDTO>(); ;
            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.OK);
                secondPageItems = _apiTestServerFixture.GetContent<List<ProductDetailsDTO>>(message.Content);
            });

            Assert.False(firstPageItems.Equals(secondPageItems));

            searchRequest = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "Search/?query=aspirin&pageSize=1&pageFrom=2"));

            _apiTestServerFixture.SendRequest(searchRequest, message =>
            {
                Assert.True(message.StatusCode == HttpStatusCode.NotFound);
            });
        }



        public void Dispose()
        {
            var removeReques = _apiTestServerFixture.CreateGetRequest(string.Format(BaseUrl, "RemoveIndex"));
            _apiTestServerFixture.SendRequest(removeReques, message => { });
        }
    }
}
