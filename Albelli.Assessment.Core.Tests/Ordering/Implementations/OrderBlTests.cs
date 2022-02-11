using Albelli.Assessment.Core.Ordering.Implementations;
using Albelli.Assessment.Domain.Enums;
using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.Infrastructure.Ordering.Interfaces;
using Moq;
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
            _orderBl = new OrderBl(_mockIOrderDal.Object);

            // Act
            var requiredBinWidth = await _orderBl.PlaceOrder(order);

            // Assert
            Assert.Equal(242.4m, requiredBinWidth);
        }
    }
}