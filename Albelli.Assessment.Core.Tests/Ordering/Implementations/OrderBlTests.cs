using Albelli.Assessment.Core.Ordering.Implementations;
using Albelli.Assessment.Domain.Enums;
using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.Infrastructure.Ordering.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Albelli.Assessment.Core.Tests.Ordering.Implementations
{
    public class OrderBlTests
    {
        private readonly Mock<IOrderDal> _mockIOrderDal;
        private OrderBl _orderBl;

        public OrderBlTests()
        {
            _mockIOrderDal = new Mock<IOrderDal>();
        }

        /// <summary>
        /// Check that the required bin width is returned correctly when the order has: 1 PhotoBook, 2 Calendars, 1 Mug.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_1_PhotoBook_2_Calendars_1_Mug()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 2 },
                    new() { ProductType = ProductType.Mug, Quantity = 1 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);

            // Assert
            Assert.Equal(133m, requiredBinWidth);
        }

        /// <summary>
        /// Check that the required bin width is returned correctly when the order has: 1 PhotoBook, 2 Calendars, 2 Mugs.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_1_PhotoBook_2_Calendars_2_Mugs()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 2 },
                    new() { ProductType = ProductType.Mug, Quantity = 2 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);

            // Assert
            Assert.Equal(133m, requiredBinWidth);
        }

        /// <summary>
        /// Check that the required bin width is returned correctly when the order has: 1 PhotoBook, 1 Calendar, 1 Canvas, 1 Cards, 1 Mug.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_1_Of_Each_Product_Type()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 1 },
                    new() { ProductType = ProductType.Canvas, Quantity = 1 },
                    new() { ProductType = ProductType.Cards, Quantity = 1 },
                    new() { ProductType = ProductType.Mug, Quantity = 1 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);
            
            // Assert
            Assert.Equal(143.7m, requiredBinWidth);
        }

        /// <summary>
        /// Check that the required bin width is returned correctly when the order has: 1 PhotoBook, 1 Calendar, 1 Canvas, 1 Cards, 4 Mugs.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_1_Of_Each_Product_Type_But_4_Mugs()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 1 },
                    new() { ProductType = ProductType.Canvas, Quantity = 1 },
                    new() { ProductType = ProductType.Cards, Quantity = 1 },
                    new() { ProductType = ProductType.Mug, Quantity = 4 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);

            // Assert
            Assert.Equal(143.7m, requiredBinWidth);
        }

        /// <summary>
        /// Check that the required bin width is returned correctly when the order has: 1 PhotoBook, 1 Calendar, 1 Canvas, 1 Cards, 5 Mugs.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_1_Of_Each_Product_Type_But_5_Mugs()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 1 },
                    new() { ProductType = ProductType.Canvas, Quantity = 1 },
                    new() { ProductType = ProductType.Cards, Quantity = 1 },
                    new() { ProductType = ProductType.Mug, Quantity = 5 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);

            // Assert
            Assert.Equal(237.7m, requiredBinWidth);
        }

        /// <summary>
        /// Check that the required bin width is returned correctly when the order has: 1 PhotoBook, 1 Calendar, 1 Canvas, 2 Cards, 5 Mugs.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_1_Of_Each_Product_Type_But_2_Cards_And_5_Mugs()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 1 },
                    new() { ProductType = ProductType.Canvas, Quantity = 1 },
                    new() { ProductType = ProductType.Cards, Quantity = 2 },
                    new() { ProductType = ProductType.Mug, Quantity = 5 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);

            // Assert
            Assert.Equal(242.4m, requiredBinWidth);
        }

        /// <summary>
        /// If the order has an invalid order id validate that an exception is thrown.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_Invalid_Order_Id()
        {
            var order = new Order
            {
                Id = 0
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _orderBl.PlaceOrder(order));
            Assert.Equal("The order number is invalid, it cannot be 0 or less.", exception.Message);
        }

        /// <summary>
        /// If the order has no products validate that an exception is thrown.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_No_Products()
        {
            var order = new Order
            {
                Id = 1
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _orderBl.PlaceOrder(order));
            Assert.Equal("The order has no products and cannot be added to the system.", exception.Message);
        }

        /// <summary>
        /// If the order has a product with 0 quantity validate that an exception is thrown.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_Products_With_0_Quantity()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 1 },
                    new() { ProductType = ProductType.Canvas, Quantity = 0 },
                    new() { ProductType = ProductType.Cards, Quantity = 1 },
                    new() { ProductType = ProductType.Mug, Quantity = 1 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>()));
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _orderBl.PlaceOrder(order));
            Assert.Equal("The order has products with 0 quantity and cannot be added to the system.", exception.Message);
        }

        /// <summary>
        /// If the order already exists validate that an exception is thrown.
        /// </summary>
        [Fact]
        public async Task PlaceOrder_Existing_Order_Id()
        {
            const int orderId = 1;

            var order = new Order
            {
                Id = orderId,
                Products = new List<Product>
                {
                    new() { ProductType = ProductType.PhotoBook, Quantity = 1 },
                    new() { ProductType = ProductType.Calendar, Quantity = 1 },
                    new() { ProductType = ProductType.Canvas, Quantity = 1 },
                    new() { ProductType = ProductType.Cards, Quantity = 1 },
                    new() { ProductType = ProductType.Mug, Quantity = 1 }
                }
            };

            // Arrange
            _mockIOrderDal.Setup(x => x.CreateOrder(order));
            _mockIOrderDal.Setup(x => x.InsertProducts(order.Products));
            _mockIOrderDal.Setup(x => x.GetOrder(It.IsAny<int>())).ReturnsAsync(new Order { Id = orderId });
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _orderBl.PlaceOrder(order));
            Assert.Equal("The order number is already in the system.", exception.Message);
        }
    }
}