using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services
{
    public class StocksService : IStocksService
    {

        private readonly IStocksRepository _stocksRepository;
        public StocksService(IStocksRepository stocksRepository) 
        {
            _stocksRepository = stocksRepository;
        }
        /// <summary>
        /// Create buy order
        /// </summary>
        /// <param name="buyOrderRequest">buy order request</param>
        /// <returns>Buy order response</returns>
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            ValidationHelper.ModelValidation(buyOrderRequest);

            DateTime minDate = Convert.ToDateTime("2000-01-01");

            if (buyOrderRequest.DateAndTimeOfOrder < minDate)
            {
                throw new ArgumentException("Order datetime should be equal or newer date than 2000-01-01");
            }

            // Save buy order
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            BuyOrder buyOrderFromAdded = await _stocksRepository.CreateBuyOrder(buyOrder);

            // return buy order response
            BuyOrderResponse buyOrderResponse = buyOrderFromAdded.ToBuyOrderResponse();

            return buyOrderResponse;
        }

        /// <summary>
        /// Create sell order
        /// </summary>
        /// <param name="sellOrderRequest">sell order request</param>
        /// <returns>Sell order response</returns>
        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            ValidationHelper.ModelValidation(sellOrderRequest);

            DateTime minDate = Convert.ToDateTime("2000-01-01");

            if (sellOrderRequest.DateAndTimeOfOrder < minDate)
            {
                throw new ArgumentException("Order datetime should be equal or newer date than 2000-01-01");
            }

            // Save sell order
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            SellOrder sellOrderFromAdded = await _stocksRepository.CreateSellOrder(sellOrder);

             // return sell order response
             SellOrderResponse sellOrderResponse = sellOrderFromAdded.ToSellOrderResponse();

            return sellOrderResponse;
        }

        /// <summary>
        /// Get list of buy order
        /// </summary>
        /// <returns>Return buy order response as list of buy order</returns>
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders(); 

            List <BuyOrderResponse> buyOrdersResponse = new List<BuyOrderResponse> ();
            foreach (BuyOrder buyOrder in buyOrders)
            {
                buyOrdersResponse.Add(buyOrder.ToBuyOrderResponse());
            }
            return buyOrdersResponse;
        }

        /// <summary>
        /// Get list of sell order
        /// </summary>
        /// <returns>Return sell order response as list of sell order</returns>
        public  async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();

            List<SellOrderResponse> sellOrdersResponse = new List<SellOrderResponse>();
            foreach (var sellOrder in sellOrders)
            {
                sellOrdersResponse.Add(sellOrder.ToSellOrderResponse());
            }
            return sellOrdersResponse;
        }

        /// <summary>
        /// Get list of both buy and sell order
        /// </summary>
        /// <returns>Return tradeOrders response that present for all data of both to table are BuyOrder and SellOrder</returns>
        public async Task<List<TradeOrderResponse>> GetTradeOrders()
        {
            List<TradeOrderResponse> tradeOrdersResponse = new List<TradeOrderResponse>();
            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();

            foreach (BuyOrder buyOrder in buyOrders)
            {
                tradeOrdersResponse.Add(buyOrder.ToTradeOrderResponse());
            }

            foreach (var sellOrder in sellOrders)
            {
                tradeOrdersResponse.Add(sellOrder.ToTradeOrderResponse());
            }

            return tradeOrdersResponse;
        }
    }
}
