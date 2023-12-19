using System.ComponentModel.DataAnnotations;

namespace TestFinnhub.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        
        [Range(1, 100000, ErrorMessage = "Minimum Quantity is 1 and maximum is 10000")]
        public uint Quantity { get; set; }
        public double Price { get; set; }

    }
}
