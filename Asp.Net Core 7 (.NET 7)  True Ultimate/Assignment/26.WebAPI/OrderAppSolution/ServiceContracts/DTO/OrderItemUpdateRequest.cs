using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents a request object will be update to order item
    /// </summary>
    public class OrderItemUpdateRequest
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


        /// <summary>
        /// Converts the <see cref="OrderItemUpdateRequest"/> object to an <see cref="OrderItem"/> object.
        /// </summary>
        /// <returns>The converted <see cref="OrderItem"/> object.</returns>
        public OrderItem ToOrderItem()
        {
            return new OrderItem
            {
                OrderItemId = OrderItemId,
                OrderId = OrderId,
                ProductName = ProductName,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                TotalPrice = TotalPrice
            };
        }
    }
}
