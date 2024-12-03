using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represent a service of order
    /// </summary>
    public interface IOrdersService
    {

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>A list of orders</returns>
        Task<List<OrderResponse>> GetAllOrders();

        /// <summary>
        /// Get an order by specify ID
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The retrieved order, or null if not found.</returns>
        Task<OrderResponse?> GetOrderByOrderId(Guid orderId);

        /// <summary>
        /// Adds a new order
        /// </summary>
        /// <param name="orderRequest">The order from customer</param>
        /// <returns>The added order</returns>
        Task<OrderResponse> AddOrder(OrderAddRequest orderRequest);

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="orderRequest">The updated order details.</param>
        /// <returns>The updated order.</returns>
        Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderRequest);

        /// <summary>
        /// Deletes an order by ID
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns>True: success, False: fail</returns>
        Task<bool> DeleteOrderByOrderId(Guid orderId);

    }
}
