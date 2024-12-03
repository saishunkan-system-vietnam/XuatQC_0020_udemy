using Castle.Core.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Represents an item of an order
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Unique identifier of the order item
        /// </summary>
        [Key]
        public Guid OrderItemId { get; set; }


        /// <summary>
        /// Unique identifier of the order related to the order item.
        /// </summary>
        [Required(ErrorMessage = "The Order Id is required field")]
        public Guid OrderId { get; set; }


        /// <summary>
        /// Name of the product related to the order item
        /// </summary>
        [Required(ErrorMessage = "The product name is required field")]
        [StringLength(50, ErrorMessage = "The product name must under 50 characters.")]
        public string? ProductName { get; set; }


        /// <summary>
        /// Quantity of the product in the order item
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity must be a positive number")]
        public int Quantity { get; set; }


        /// <summary>
        /// Unit price of the product in the order item
        /// </summary>
        [Range(0, 100000000, ErrorMessage = "The UnitPrice must be a positive number")]
        [Column(TypeName = "decimal")]
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// Total price of the order item
        /// </summary>
        [Range(0, 100000000, ErrorMessage = "The total price of the order item must be a positive number")]
        [Column(TypeName = "decimal")]
        public decimal TotalPrice { get; set; }

        public virtual Order Orders { get; set; }

    }
}

