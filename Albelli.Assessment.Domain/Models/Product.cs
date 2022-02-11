using Albelli.Assessment.Domain.Enums;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Albelli.Assessment.Domain.Models
{
    public class Product
    {
        /// <summary>
        /// The order id.
        /// </summary>
        [ForeignKey(typeof(Order))]
        public int OrderId { get; set; }

        /// <summary>
        /// The type of product.
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// The quantity of products.
        /// </summary>
        public int Quantity { get; set; }
    }
}
