using Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents a response of an order
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        /// ID of the order
        /// </summary>
        public Guid OrderId { get; set; }


        /// <summary>
        /// Order number
        /// </summary>
        public string? OrderNumber { get; set; }


        /// <summary>
        /// Name of the customer associated with the order
        /// </summary>
        public string? CustomerName { get; set; }


        /// <summary>
        /// Date of the order
        /// </summary>
        public DateTime OrderDate { get; set; }


        /// <summary>
        /// Total amount of the order
        /// </summary>
        public decimal TotalAmount { get; set; }


        /// <summary>
        /// the list of order items related to the order
        /// </summary>
        public List<OrderItemResponse>? OrderItems { get; set; } = new List<OrderItemResponse>();


        /// <summary>
        /// Converts the OrderResponseobject to an Order
        /// </summary>
        /// <returns>The converted Order</returns>
        public Order ToOrder()
        {
            return new Order
            {
                OrderId = OrderId,
                OrderNumber = OrderNumber,
                CustomerName = CustomerName,
                OrderDate = (DateTime?)OrderDate,
                TotalAmount = TotalAmount
            };
        }
    }


    /// <summary>
    /// Provides extension methods for <Order"/> objects.
    /// </summary>
    public static class OrderExtensions
    {
        /// <summary>
        /// Converts an <Order"/> object to an <OrderResponse"/> object.
        /// </summary>
        /// <param name="order">The <Order"/> object to convert.</param>
        /// <returns>An <OrderResponse"/> object.</returns>
        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                OrderDate = (DateTime)order.OrderDate,
                TotalAmount = order.TotalAmount
            };
        }

        public static OrderResponse ToOrderResponseWithItemResponse(this Order order)
        {
            List<OrderItemResponse>? orderItems = null;

            if (order.OrderItems != null)
            {
                orderItems = new List<OrderItemResponse>();
                foreach (var orderItem in order.OrderItems)
                {
                    orderItems.Add(orderItem.ToOrderItemResponse());
                }
            }
            return new OrderResponse
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                OrderDate = (DateTime)order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = orderItems
            };
        }

        /// <summary>
        /// Converts a list of <Order"/> objects to a list of <OrderResponse"/> objects.
        /// </summary>
        /// <param name="orders">The list of <Order"/> objects to convert.</param>
        /// <returns>A list of <OrderResponse"/> objects.</returns>
        public static List<OrderResponse> ToOrderResponseList(this List<Order> orders)
        {
            var orderResponses = new List<OrderResponse>();
            foreach (var order in orders)
            {
                orderResponses.Add(order.ToOrderResponse());
            }
            return orderResponses;
        }
    }

}
