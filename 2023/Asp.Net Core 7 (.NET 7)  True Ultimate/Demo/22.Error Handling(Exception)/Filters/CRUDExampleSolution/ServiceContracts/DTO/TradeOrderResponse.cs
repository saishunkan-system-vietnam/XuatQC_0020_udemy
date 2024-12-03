using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class return type for TradeOrderResponse(both of buy and sell orders) most of Finhubservice methods
    /// </summary>
    public class TradeOrderResponse
    {
        public Guid OrderID { get; set; }

        public string StockSymbol { get; set; } = string.Empty;

        public string StockName { get; set; } = string.Empty;

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public string OrderType { get; set; } = string.Empty;

    }

    public static class TradeOrderResponseExtension
    {
        public static TradeOrderResponse ToTradeOrderResponse(this SellOrder sellOrder)
        {
            return new TradeOrderResponse()
            {
                OrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                OrderType = OrderTypeOptions.Sell.ToString(),
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }

        public static TradeOrderResponse ToTradeOrderResponse(this BuyOrder buyOrder)
        {
            return new TradeOrderResponse()
            {
                OrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                OrderType = OrderTypeOptions.Buy.ToString(),
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
