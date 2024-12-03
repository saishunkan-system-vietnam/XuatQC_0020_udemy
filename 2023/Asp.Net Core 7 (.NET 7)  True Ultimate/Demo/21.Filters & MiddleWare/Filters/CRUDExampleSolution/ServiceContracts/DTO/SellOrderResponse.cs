using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class return type for SellOrder most of Finhubservice methods
    /// </summary>
    public class SellOrderResponse : IOrderResponse
    {
        public Guid SellOrderID { get; set; }

        public string StockSymbol { get; set; } = string.Empty;

        public string StockName { get; set; } = string.Empty;

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(SellOrderResponse)) return false;

           SellOrderResponse sellOrderResponse = (SellOrderResponse)obj;

            return SellOrderID == sellOrderResponse.SellOrderID &&
                StockSymbol == sellOrderResponse.StockSymbol &&
                StockName == sellOrderResponse.StockName &&
                Price == sellOrderResponse.Price &&
                Quantity == sellOrderResponse.Quantity &&
                TradeAmount == sellOrderResponse.TradeAmount &&
                DateAndTimeOfOrder == sellOrderResponse.DateAndTimeOfOrder;
        }
    }

    public static class SellOrderResponseExtension
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
