using Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents a request object will be update to order
    /// </summary>
    public class OrderUpdateRequest
    {
        /// <summary>
        /// ID of the order.
        /// </summary>
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
        [Range(0, double.MaxValue, ErrorMessage = "The Total amount must be a positive number")]
        [Column(TypeName = "decimal")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Convert data from the OrderUpdateRequest to Order
        /// </summary>
        /// <returns>An Order</returns>
        public Order ToOrder()
        {
            return new Order
            {
                OrderId = OrderId,
                CustomerName = CustomerName,
                OrderNumber = OrderNumber,
                OrderDate = OrderDate,
                TotalAmount = TotalAmount,
            };
        }
    }
}
