using Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;
using System.Collections.Generic;

namespace Services
{
    public class StocksService : IStocksService
    {
        private List<BuyOrder> _buyOrders;
        private List<SellOrder> _sellOrders;
        public StocksService() 
        { 
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();

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
            _buyOrders.Add(buyOrder);

            // return buy order response
            BuyOrderResponse response = buyOrder.ToBuyOrderResponse();

            return response;
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
            _sellOrders.Add(sellOrder);

            // return sell order response
            SellOrderResponse response = sellOrder.ToSellOrderResponse();

            return response;
        }

        /// <summary>
        /// Get list of buy order
        /// </summary>
        /// <returns>Return buy order response as list of buy order</returns>
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List <BuyOrderResponse> buyOrdersResponse = new List<BuyOrderResponse> ();
            foreach (var buyOrder in _buyOrders)
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
            foreach (var sellOrder in _sellOrders)
            {
                sellOrdersResponse.Add(sellOrder.ToSellOrderResponse());
            }
            return sellOrdersResponse;
        }

    }
}
