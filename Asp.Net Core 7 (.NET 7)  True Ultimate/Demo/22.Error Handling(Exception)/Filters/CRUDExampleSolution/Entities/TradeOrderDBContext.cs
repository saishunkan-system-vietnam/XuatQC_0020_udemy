using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class TradeOrderDBContext: DbContext
    {
        public TradeOrderDBContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrder");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrder");
        }
    }
}
