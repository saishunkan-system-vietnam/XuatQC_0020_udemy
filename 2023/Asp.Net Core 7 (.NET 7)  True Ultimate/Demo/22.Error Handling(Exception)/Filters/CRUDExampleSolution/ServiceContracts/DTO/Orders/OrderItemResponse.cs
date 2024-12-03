using Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents a response for an order item
    /// </summary>
    public class OrderItemResponse
    {
        /// <summary>
        /// ID of the order item
        /// </summary>
        public Guid OrderItemId { get; set; }


        /// <summary>
        /// ID of the order associated with the order item
        /// </summary>
        public Guid OrderId { get; set; }


        /// <summary>
        /// Name of the product associated with the order item
        /// </summary>
        public string? ProductName { get; set; }


        /// <summary>
        /// Quantity of the product in the order item
        /// </summary>
        public int Quantity { get; set; }


        /// <summary>
        /// Unit price of the product in the order item
        /// </summary>
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// Total price of the order item
        /// </summary>
        public decimal TotalPrice { get; set; }
    }


    /// <summary>
    /// Provides extension methods for OrderItem
    /// </summary>
    public static class OrderItemResponseExtensions
    {
        /// <summary>
        /// Converts an OrderItem to an OrderItemResponse
        /// </summary>
        /// <param name="orderItem">The OrderItem to convert.</param>
        /// <returns>An OrderItemResponse</returns>
        public static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
        {
            return new OrderItemResponse
            {
                OrderItemId= orderItem.OrderItemId,
                OrderId= orderItem.OrderId,
                ProductName= orderItem.ProductName,
                Quantity= orderItem.Quantity,
                UnitPrice= orderItem.UnitPrice,
                TotalPrice= orderItem.TotalPrice
            };
        }


        /// <summary>
        /// Converts a list of OrderItem to a list of OrderItemResponse
        /// </summary>
        /// <param name="orderItems">The list of OrderItem to convert</param>
        /// <returns>A list of OrderItemResponse</returns>
        public static List<OrderItemResponse> ToOrderItemResponseList(this List<OrderItem> orderItems)
        {
            var orderItemResponses = new List<OrderItemResponse>();
            foreach (var orderItem in orderItems)
            {
                orderItemResponses.Add(orderItem.ToOrderItemResponse());
            }
            return orderItemResponses;
        }
    }
}

