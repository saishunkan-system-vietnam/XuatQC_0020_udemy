using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents a request object will be add to order item
    /// </summary>
    public class OrderItemAddRequest
    {
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
        /// Converts OrderItem object.
        /// </summary>
        /// <returns>OrderItem object.</returns>
        public OrderItem ToOrderItem()
        {
            return new OrderItem
            {
                OrderId = OrderId,
                ProductName = ProductName,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                TotalPrice = TotalPrice
            };
        }
    }



    public static class OrderItemAddRequestExtensions
    {
        /// <summary>
        /// Converts a list of OrderItemAddRequest to a list of OrderItem
        /// </summary>
        /// <param name="orderItemRequests">The list of OrderItemAddRequest</param>
        /// <returns>A list of OrderItem </returns>
        public static List<OrderItem> ToOrderItems(this List<OrderItemAddRequest> orderItemRequests)
        {
            var orderItems = new List<OrderItem>();
            foreach (var orderItemRequest in orderItemRequests)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderItemRequest.OrderId,
                    ProductName = orderItemRequest.ProductName,
                    Quantity = orderItemRequest.Quantity,
                    UnitPrice = orderItemRequest.UnitPrice,
                    TotalPrice = orderItemRequest.TotalPrice
                };

                orderItems.Add(orderItem);
            }

            return orderItems;
        }
    }

}
