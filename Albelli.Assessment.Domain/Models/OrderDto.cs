namespace Albelli.Assessment.Domain.Models
{
    public class OrderDto
    {
        /// <summary>
        /// The order id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The products in the order.
        /// </summary>
        public List<ProductDto>? Products { get; set; }

        /// <summary>
        /// The minimum bin width.
        /// </summary>
        public decimal RequiredBinWidth { get; set; }
    }
}
