using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class OrderDbContext: DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// DbSet of the orders.
        /// </summary>
        public virtual DbSet<Order> Orders { get; set; }


        /// <summary>
        /// DbSet of the order items.
        /// </summary>
        public virtual DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
        }
    }
}
