using Entities;
using ServiceContracts.Enums;
using System.Net;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class return type for BuyOrder most of Finhubservice methods
    /// </summary>
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }

        public string StockSymbol { get; set; } = string.Empty;

        public string StockName { get; set; } = string.Empty;

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(BuyOrderResponse)) return false;

            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;

            return BuyOrderID == buyOrderResponse.BuyOrderID &&
                StockSymbol == buyOrderResponse.StockSymbol &&
                StockName == buyOrderResponse.StockName &&
                Price == buyOrderResponse.Price &&
                Quantity == buyOrderResponse.Quantity &&
                TradeAmount == buyOrderResponse.TradeAmount &&
                DateAndTimeOfOrder == buyOrderResponse.DateAndTimeOfOrder;
        }

    }

    public static class BuyOrderResponseExtension
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
