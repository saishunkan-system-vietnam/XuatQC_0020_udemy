using Entities;
using RepositoryContracts;

namespace Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly TradeOrderDBContext _dbContext;
        public StockRepository(TradeOrderDBContext tradeOrderDBContext)
        {
            _dbContext = tradeOrderDBContext;

        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder? buyOrder)
        {
            // Save buy order
            buyOrder.BuyOrderID = Guid.NewGuid();

            _dbContext.BuyOrders.Add(buyOrder);
            await _dbContext.SaveChangesAsync();

            return buyOrder;

        }

        public async Task<SellOrder> CreateSellOrder(SellOrder? sellOrder)
        {
            // Save sell order
            sellOrder.SellOrderID = Guid.NewGuid();

            _dbContext.SellOrders.Add(sellOrder);
            await _dbContext.SaveChangesAsync();

            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return _dbContext.BuyOrders.ToList();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return _dbContext.SellOrders.ToList();
        }
    }
}
