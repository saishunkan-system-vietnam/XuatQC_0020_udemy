using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents the repository contract for order
    /// </summary>
    public interface IOrdersRepository
    {
        /// <summary>
        /// Adds an order to the repository
        /// </summary>
        /// <param name="order">The order to add</param>
        /// <returns>The added order</returns>
        Task<Order> AddOrder(Order order);


        /// <summary>
        /// Deletes an order from the repository by specfic order ID
        /// </summary>
        /// <param name="orderId">The ID of the order to delete</param>
        /// <returns>True: delete success, False: delete fail</returns>
        Task<bool> DeleteOrderByOrderId(Guid orderId);


        /// <summary>
        /// Get all orders from the repository
        /// </summary>
        /// <returns>A list of orders</returns>
        Task<List<Order>> GetAllOrders();


        /// <summary>
        /// Get an order from the repository based on its order ID
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <returns>The order matching the order ID, or null if not found</returns>
        Task<Order?> GetOrderByOrderId(Guid orderId);


        /// <summary>
        /// Updates an order in the repository
        /// </summary>
        /// <param name="order">The updated order</param>
        /// <returns>The updated order</returns>
        Task<Order> UpdateOrder(Order order);

    }
}
