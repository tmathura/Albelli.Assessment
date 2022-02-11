using Albelli.Assessment.Domain.Enums;

namespace Albelli.Assessment.Domain.Models
{
    public class ProductDto
    {
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
