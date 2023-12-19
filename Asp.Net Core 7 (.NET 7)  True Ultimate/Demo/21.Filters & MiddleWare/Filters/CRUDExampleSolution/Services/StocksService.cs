using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly IStockRepository _stockRepository;
        public StocksService(IStockRepository stockRepository) 
        {
            _stockRepository = stockRepository;
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
            BuyOrder buyOrderRespone = await _stockRepository.CreateBuyOrder(buyOrder);

            // return buy order response
            return buyOrderRespone.ToBuyOrderResponse();
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
            SellOrder sellOrderResponse = await _stockRepository.CreateSellOrder(sellOrder);

            // return sell order response
            return sellOrderResponse.ToSellOrderResponse();
        }

        /// <summary>
        /// Get list of buy order
        /// </summary>
        /// <returns>Return buy order response as list of buy order</returns>
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List <BuyOrderResponse> buyOrdersResponse = new List<BuyOrderResponse> ();

            List<BuyOrder> lstBuyOrder = await _stockRepository.GetBuyOrders();

            foreach (var buyOrder in lstBuyOrder)
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
            List<SellOrderResponse> sellOrdersResponse = new List<SellOrderResponse>();
            List<SellOrder> lstSellOrder = await _stockRepository.GetSellOrders();

            foreach (var sellOrder in lstSellOrder)
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

            List<BuyOrder> lstBuyOrder = await _stockRepository.GetBuyOrders();
            List<SellOrder> lstSellOrder = await _stockRepository.GetSellOrders();

            foreach (BuyOrder buyOrder in lstBuyOrder)
            {
                tradeOrdersResponse.Add(buyOrder.ToTradeOrderResponse());
            }

            foreach (var sellOrder in lstSellOrder)
            {
                tradeOrdersResponse.Add(sellOrder.ToTradeOrderResponse());
            }

            return tradeOrdersResponse;
        }
    }
}
