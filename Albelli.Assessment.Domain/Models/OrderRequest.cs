using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Albelli.Assessment.Domain.Models
{
    public class OrderRequest
    {
        /// <summary>
        /// The order id.
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// The products in the order.
        /// </summary>
        [OneToMany]
        public List<ProductRequest>? Products { get; set; }
    }
}
