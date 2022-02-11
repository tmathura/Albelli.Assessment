using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.Infrastructure.Ordering.Interfaces;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace Albelli.Assessment.Infrastructure.Ordering.Implementations
{
    /// <summary>
    /// Ordering data access logic
    /// </summary>
    /// <seealso cref="IOrderDal" />
    public class OrderDal : IOrderDal
    {
        private readonly SQLiteAsyncConnection _database;

        public OrderDal(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<Order>().Wait();
            _database.CreateTableAsync<Product>().Wait();
        }

        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to create.</param>
        public async Task CreateOrder(Order order)
        {
            await _database.InsertAsync(order);
        }
        
        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="id">Id of order to lookup.</param>
        /// <returns><see cref="Order"/></returns>
        public async Task<Order> GetOrder(int id)
        {
            return await _database.Table<Order>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert order products.
        /// </summary>
        /// <param name="products"><see cref="Product"/>s in the order.</param>
        public async Task InsertProducts(List<Product>? products)
        {
            await _database.InsertAllAsync(products);
        }

        /// <summary>
        /// Get order products.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to get products.</param>
        public async Task GetProducts(Order order)
        {
            if (order != null)
            {
                await _database.GetChildrenAsync(order);
            }
        }
    }
}
