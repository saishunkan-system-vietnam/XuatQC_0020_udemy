using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemsRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
        {
            orderItem.OrderItemId = Guid.NewGuid();
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
            {
                return false;
            }

            _context.OrderItems.Remove(orderItem);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            return orderItem;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId)
        {
            var orderItem = await _context.OrderItems.Where(item => item.OrderId == orderId).ToListAsync();
            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }
    }
}
