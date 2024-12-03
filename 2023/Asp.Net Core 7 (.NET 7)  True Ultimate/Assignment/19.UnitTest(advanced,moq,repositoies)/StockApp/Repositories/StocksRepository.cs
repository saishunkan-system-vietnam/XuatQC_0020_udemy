using Entities;
using Newtonsoft.Json;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly TradeOrderDBContext _dbContext;
        public StocksRepository(TradeOrderDBContext tradeOrderDBContext)
        {
            _dbContext = tradeOrderDBContext;

        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _dbContext.BuyOrders.Add(buyOrder);
            await _dbContext.SaveChangesAsync();

            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
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