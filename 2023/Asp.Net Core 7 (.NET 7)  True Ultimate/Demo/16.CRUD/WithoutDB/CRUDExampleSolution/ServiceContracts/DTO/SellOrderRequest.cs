using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for SellOrder class
    /// </summary>
    public class SellOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can not be null or blank")]
        public string StockSymbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stock name can not be null or blank")]
        public string StockName { get; set; } = string.Empty;

        public DateTime DateAndTimeOfOrder { get; set; }


        [Range(1, 100000, ErrorMessage = "Minimum quantity is 1 and maximum is 100000")]
        public uint Quantity { get; set; }


        [Range(1, 10000, ErrorMessage = "Minimum price is 1 and maximum is 10000")]
        public double Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }
    }
}
