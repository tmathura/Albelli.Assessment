using Albelli.Assessment.Domain.Models;
using Albelli.Assessment.Infrastructure.Ordering.Interfaces;
using log4net;
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
        private readonly ILog _logger = LogManager.GetLogger(typeof(OrderDal));

        public OrderDal(string databasePath)
        {
            try
            {
                _logger.Info("Set up database connection.");

                _database = new SQLiteAsyncConnection(databasePath);
                _database.CreateTableAsync<Order>().Wait();
                _database.CreateTableAsync<Product>().Wait();

                _logger.Info("Set up database connection completed.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Create order.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to create.</param>
        public async Task CreateOrder(Order order)
        {
            try
            {
                _logger.Info("Insert order into database.");

                await _database.InsertAsync(order);

                _logger.Info("Insert order into database completed.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }
        
        /// <summary>
        /// Get order by id.
        /// </summary>
        /// <param name="id">Id of order to lookup.</param>
        /// <returns><see cref="Order"/></returns>
        public async Task<Order> GetOrder(int id)
        {
            try
            {
                _logger.Info("Get order from database.");

                var order = await _database.Table<Order>().Where(i => i.Id == id).FirstOrDefaultAsync();

                _logger.Info("Get order from database completed.");
                return order;
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Insert order products.
        /// </summary>
        /// <param name="products"><see cref="Product"/>s in the order.</param>
        public async Task InsertProducts(List<Product>? products)
        {
            try
            {
                _logger.Info("Insert products into database.");

                await _database.InsertAllAsync(products);

                _logger.Info("Insert products into database completed.");
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Get order products.
        /// </summary>
        /// <param name="order"><see cref="Order"/> to get products.</param>
        public async Task GetProducts(Order order)
        {
            try
            {
                if (order != null)
                {
                    _logger.Info("Get products from database.");

                    await _database.GetChildrenAsync(order);

                    _logger.Info("Get products from database completed.");
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"{exception.Message} - {exception.StackTrace}");
                throw;
            }
        }
    }
}
