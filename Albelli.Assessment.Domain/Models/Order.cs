using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Albelli.Assessment.Domain.Models
{
    public class Order
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
        public List<Product>? Products { get; set; }

        /// <summary>
        /// The minimum bin width.
        /// </summary>
        public decimal RequiredBinWidth { get; set; }

        /// <summary>
        /// The date the order was created.
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
