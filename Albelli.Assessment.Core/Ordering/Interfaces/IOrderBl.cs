using Albelli.Assessment.Domain.Models;

namespace Albelli.Assessment.Core.Ordering.Interfaces
{
    public interface IOrderBl
    {
        /// <summary>
        /// Place order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to create.</param>
        /// <returns>RequiredBinWidth</returns>
        Task<int> PlaceOrder(Order order);

        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="id">Id of order to lookup.</param>
        /// <returns><see cref="Order"/></returns>
        Task<Order> GetOrder(int id);
    }
}