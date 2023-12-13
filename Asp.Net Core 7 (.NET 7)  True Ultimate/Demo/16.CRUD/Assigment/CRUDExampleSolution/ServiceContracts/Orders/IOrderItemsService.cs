using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represnt the service of Order item
    /// </summary>
    public interface IOrderItemsService
    {
        /// <summary>
        /// Adds an order item
        /// </summary>
        /// <param name="orderItemRequest">The order item data from client request</param>
        /// <returns>The added order item</returns>
        Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest);

        /// <summary>
        /// Deletes an order item by specific order item ID
        /// </summary>
        /// <param name="orderItemId">The ID of the order item to delete</param>
        /// <returns>True: delete success, False: Delete false</returns>
        Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId);

        /// <summary>
        /// Get all order items
        /// </summary>
        /// <returns>A list of order items</returns>
        Task<List<OrderItemResponse>> GetAllOrderItems();

        /// <summary>
        /// Get order items with a specific order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <returns>A list of order items related with order by order ID.</returns>
        Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId);

        /// <summary>
        /// Get an order item by specific order item ID.
        /// </summary>
        /// <param name="orderItemId">The ID of the order item.</param>
        /// <returns>The order item matching the order item ID, or null if not found.</returns>
        Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId);

        /// <summary>
        /// Updates an order item
        /// </summary>
        /// <param name="orderItemRequest">The updated order item data from client request</param>
        /// <returns>The updated order item</returns>
        Task<OrderItemResponse?> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest);
    }
}
