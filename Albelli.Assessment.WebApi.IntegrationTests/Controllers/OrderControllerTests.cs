using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.WebApi.IntegrationTests.Common.Helpers;
using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Albelli.Assessment.WebApi.IntegrationTests.Controllers
{
    [Collection(nameof(CommonHelper))]
    public class OrderControllerTests
    {
        private readonly CommonHelper _commonHelper;

        public OrderControllerTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
        {
            commonHelper.OutputHelper = outputHelper;
            _commonHelper = commonHelper;

            if (commonHelper.Settings.WebApi.ClearDatabase)
            {
                var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Albelli.Assessment.WebApi.SQLite.db3");
                databasePath = databasePath.Replace("Albelli.Assessment.WebApi.IntegrationTests", "Albelli.Assessment.WebApi");

                if (File.Exists(databasePath))
                {
                    var database = new SQLiteAsyncConnection(databasePath);

                    database.DropTableAsync<Order>().Wait();
                    database.DropTableAsync<Product>().Wait();
                    database.CreateTableAsync<Order>().Wait();
                    database.CreateTableAsync<Product>().Wait();
                }
            }
        }

        /// <summary>
        /// Create a new order with 1 PhotoBook and validate that the required bin width is 19mm.
        /// </summary>
        [Fact]
        public async Task PlaceOrder()
        {
            // Arrange
            var response = await _commonHelper.GetAuth0BearerToken();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(tokenResponse);

            var bearer = tokenResponse.token_type;
            var token = tokenResponse.access_token;

            Assert.NotNull(bearer);
            Assert.NotNull(token);

            var donationBody = new
            {
                id = 10,
                products = new[]
                {
                    new
                    {
                        productType =  0,
                        quantity = 1
                    }

                }
            };

            // Act
            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic placeOrderResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.NotNull(placeOrderResponse);
            Assert.Equal(19, placeOrderResponse);
        }

        /// <summary>
        /// Create a new order with an invalid order number and validate that the response status code is InternalServerError.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_Invalid_Order_Id()
        {
            // Arrange
            var response = await _commonHelper.GetAuth0BearerToken();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(tokenResponse);

            var bearer = tokenResponse.token_type;
            var token = tokenResponse.access_token;

            Assert.NotNull(bearer);
            Assert.NotNull(token);
            var donationBody = new
            {
                id = 0,
                products = new[]
                {
                    new
                    {
                        productType =  0,
                        quantity = 1
                    }

                }
            };
            
            // Act
            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            dynamic placeOrderResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.NotNull(placeOrderResponse);
            Assert.Equal("The order number is invalid, it cannot be 0 or less.", placeOrderResponse);
        }

        /// <summary>
        /// Create a new order with no products and validate that the response status code is InternalServerError.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_No_Products()
        {
            // Arrange
            var response = await _commonHelper.GetAuth0BearerToken();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(tokenResponse);

            var bearer = tokenResponse.token_type;
            var token = tokenResponse.access_token;

            var donationBody = new
            {
                id = 0,
                products = Array.Empty<object>()
            };

            // Act
            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            dynamic placeOrderResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.NotNull(placeOrderResponse);
            Assert.Equal("The order number is invalid, it cannot be 0 or less.", placeOrderResponse);
        }

        /// <summary>
        /// Create a new order that has a product with 0 quantity and validate that the response status code is InternalServerError.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_Products_With_0_Quantity()
        {
            // Arrange
            var response = await _commonHelper.GetAuth0BearerToken();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(tokenResponse);

            var bearer = tokenResponse.token_type;
            var token = tokenResponse.access_token;

            var donationBody = new
            {
                id = 0,
                products = new[]
                {
                    new
                    {
                        productType =  0,
                        quantity = 1
                    },
                    new
                    {
                        productType =  1,
                        quantity = 0
                    }

                }
            };

            // Act
            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            dynamic placeOrderResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.NotNull(placeOrderResponse);
            Assert.Equal("The order number is invalid, it cannot be 0 or less.", placeOrderResponse);
        }

        /// <summary>
        /// Create a new order, then create a with the same order id and validate that the response status code is InternalServerError.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_Duplicate_Order_Id()
        {
            // Arrange
            var response = await _commonHelper.GetAuth0BearerToken();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(tokenResponse);

            var bearer = tokenResponse.token_type;
            var token = tokenResponse.access_token;

            const int id = 20;

            var donationBody = new
            {
                id,
                products = new[]
                {
                    new
                    {
                        productType =  0,
                        quantity = 1
                    }

                }
            };

            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Act
            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            dynamic placeOrderResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.NotNull(placeOrderResponse);
            Assert.Equal("The order number is already in the system.", placeOrderResponse);
        }

        /// <summary>
        /// Create a new order with 1 PhotoBook and validate that the order can be retrieved via the order id.
        /// </summary>
        [Fact]
        public async Task GetOrder()
        {
            // Arrange
            var response = await _commonHelper.GetAuth0BearerToken();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            Assert.NotNull(tokenResponse);

            var bearer = tokenResponse.token_type;
            var token = tokenResponse.access_token;

            const int id = 30;

            var donationBody = new
            {
                id,
                products = new[]
                {
                    new
                    {
                        productType =  0,
                        quantity = 1
                    }

                }
            };

            response = await _commonHelper.CallEndPoint("api/order", Method.Post, donationBody, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Act
            response = await _commonHelper.CallEndPoint($"api/order/{id}", Method.Get, null, bearer.ToString(), token.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            dynamic getOrderResponse = JsonConvert.DeserializeObject(response.Content);

            // Assert
            Assert.NotNull(getOrderResponse);
            Assert.Equal(id, Convert.ToInt32(getOrderResponse.id.Value));
        }
    }
}