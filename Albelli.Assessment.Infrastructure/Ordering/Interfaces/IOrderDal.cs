using Albelli.Assessment.Domain.Models;

namespace Albelli.Assessment.Infrastructure.Ordering.Interfaces
{
    public interface IOrderDal
    {
        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to create.</param>
        Task CreateOrder(Order order);

        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="id">Id of order to lookup.</param>
        /// <returns><see cref="Order"/></returns>
        Task<Order> GetOrder(int id);

        /// <summary>
        /// Insert order products.
        /// </summary>
        /// <param name="products"><see cref="Product"/>s in the order.</param>
        Task InsertProducts(List<Product>? products);

        /// <summary>
        /// Get order products.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to get products.</param>
        Task GetProducts(Order order);
    }
}