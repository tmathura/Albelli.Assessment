using Albelli.Assessment.Core.Ordering.Interfaces;
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
        public async Task<int> PlaceOrder(Order order)
        {
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
        /// <param name="products"><see cref="Product"/>s.</param>
        /// <returns>id</returns>
        private static int CalculateRequiredBinWidth(List<Product>? products)
        {
            //TODO: remove hardcoded value.
            return 165;
        }
    }
}
