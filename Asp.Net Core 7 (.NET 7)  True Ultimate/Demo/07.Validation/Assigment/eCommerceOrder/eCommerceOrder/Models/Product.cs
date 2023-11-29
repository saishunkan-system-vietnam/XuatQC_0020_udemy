using System.ComponentModel.DataAnnotations;

namespace eCommerceOrder.Models
{
    public class Product
    {
        public int ProductCode { get; set; }

        [Range(1, 999999, ErrorMessage = "Product {0} should be between valid number")]
        public double Price { get; set; }

        [Range(1, 999999, ErrorMessage = "Product {0} should be between valid number")]
        public int Quantity { get; set; }
    }
}
