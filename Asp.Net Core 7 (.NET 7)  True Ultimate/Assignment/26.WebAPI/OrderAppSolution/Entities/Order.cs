using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Represents an order entity
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Unique identifier of the order
        /// </summary>
        [Key]
        public Guid OrderId { get; set; }


        /// <summary>
        /// Name of the customer
        /// </summary>
        [Required(ErrorMessage = "The customer mame is required field")]
        [StringLength(50, ErrorMessage = "The customer name must under 50 characters")]
        public string? CustomerName { get; set; }


        /// <summary>
        /// Order number.
        /// </summary>
        [Required(ErrorMessage = "The Order number is required field")]
        [RegularExpression(@"^Order_\d{4}_\d", ErrorMessage = "The Order number should start with 'Order' followed by an underscore (_) and a sequential number")]
        public string? OrderNumber { get; set; }


        /// <summary>
        /// Date of the order
        /// </summary>
        [Required(ErrorMessage = "The order date is required field")]
        public DateTime? OrderDate { get; set; }


        /// <summary>
        /// Total amount of the order
        /// </summary>
        [Range(0, 100000000, ErrorMessage = "The Total amount must be a positive number")]
        [Column(TypeName = "decimal")]
        public decimal TotalAmount { get; set; }

    }
}

