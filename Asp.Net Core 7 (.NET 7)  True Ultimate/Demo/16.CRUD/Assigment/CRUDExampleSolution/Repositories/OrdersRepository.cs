using Entities;
using RepositoryContracts;
using System.Collections.Generic;

namespace Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrderDbContext _context;
        public OrdersRepository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<Order> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderByOrderId(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return orders;
        }

        public async Task<Order?> GetOrderByOrderId(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            return order;
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            var orderSearch = await _context.Orders.FindAsync(order.OrderId);
            if (orderSearch == null)
            {
                return null;
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
