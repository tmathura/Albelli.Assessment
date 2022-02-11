using Albelli.Assessment.Core.Ordering.Interfaces;
using Albelli.Assessment.Domain.Enums;
using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.Infrastructure.Ordering.Interfaces;

namespace Albelli.Assessment.Core.Ordering.Implementations
{
    /// <summary>
    /// Ordering business logic
    /// </summary>
    /// <seealso cref="IOrderBl" />
    public class OrderBl : IOrderBl
    {
        private const int PhotoBookWidth = 19;
        private const int CalendarWidth = 10;
        private const int CanvasWidth = 16;
        private const decimal CardsWidth = 4.7m;
        private const int MugWidth = 94;
        private const decimal MaxMugStack = 4;

        private readonly IOrderDal _orderDal;

        public OrderBl(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        /// <summary>
        /// Place order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to create.</param>
        /// <returns>RequiredBinWidth</returns>
        public async Task<decimal> PlaceOrder(Order order)
        {
            await ValidateNewOrder(order);

            var requiredBinWidth = CalculateRequiredBinWidth(order.Products);
            order.RequiredBinWidth = requiredBinWidth;
            
            await CreateOrder(order);

            return order.RequiredBinWidth;
        }
        
        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="id">Id of order to lookup.</param>
        /// <returns><see cref="Order"/></returns>
        public async Task<Order> GetOrder(int id)
        {
            var order =  await _orderDal.GetOrder(id);

            await _orderDal.GetProducts(order);

            return order;
        }

        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to create.</param>
        /// <returns>id</returns>
        private async Task CreateOrder(Order order)
        {
            order.CreateDate = DateTime.Now;
            await _orderDal.CreateOrder(order);

            var products = order.Products.Select(product => { product.OrderId = order.Id; return product; }).ToList();
            await _orderDal.InsertProducts(products);
        }

        /// <summary>
        /// Calculate the required bin width.
        /// </summary>
        /// <param name="products"><see cref="Product"/>s to calculate the required bin width.</param>
        /// <returns>id</returns>
        private static decimal CalculateRequiredBinWidth(List<Product>? products)
        {
            var photoBookQuantity = products.Where(x => x.ProductType == ProductType.PhotoBook).Sum(product => product.Quantity);
            var calendarQuantity = products.Where(x => x.ProductType == ProductType.Calendar).Sum(product => product.Quantity);
            var canvasQuantity = products.Where(x => x.ProductType == ProductType.Canvas).Sum(product => product.Quantity);
            var cardsQuantity = products.Where(x => x.ProductType == ProductType.Cards).Sum(product => product.Quantity);
            var mugQuantity = products.Where(x => x.ProductType == ProductType.Mug).Sum(product => product.Quantity);

            var photoBookBinWidth = photoBookQuantity * PhotoBookWidth;
            var calendarBinWidth = calendarQuantity * CalendarWidth;
            var canvasBinWidth = canvasQuantity * CanvasWidth;
            var cardsBinWidth = cardsQuantity * CardsWidth;
            var mugBinWidth = Math.Ceiling(mugQuantity / MaxMugStack) * MugWidth;

            var totalRequiredBinWidth = photoBookBinWidth + calendarBinWidth + canvasBinWidth + cardsBinWidth + mugBinWidth;

            return totalRequiredBinWidth;
        }

        /// <summary>
        /// Validate the new order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to validate.</param>
        /// <returns>id</returns>
        private async Task ValidateNewOrder(Order order)
        {
            if (order.Id <= 0)
            {
                throw new Exception("The order number is invalid, it cannot be 0 or less.");
            }

            if (order.Products == null || order.Products.Count == 0)
            {
                throw new Exception("The order has no products and cannot be added to the system.");
            }

            if (order.Products.Any(product => product.Quantity == 0))
            {
                throw new Exception("The order has products with 0 quantity and cannot be added to the system.");
            }

            var existingOrder = await GetOrder(order.Id);

            if (existingOrder != null)
            {
                throw new Exception("The order number is already in the system.");
            }
        }
    }
}
