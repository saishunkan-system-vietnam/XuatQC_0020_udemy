using Newtonsoft.Json;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {

        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        /// <summary>
        /// Get list profile of the company  
        /// </summary>
        /// <param name="stockSymbol">stock symbol</param>
        /// <returns>return the list profile of the company</returns>
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {

            Dictionary<string, object>? companyProfileResponse = await _finnhubRepository.GetCompanyProfile(stockSymbol);

            return companyProfileResponse;
        }

        /// <summary>
        /// Get list of the stock price
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns>return the list of stock price</returns>
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            Dictionary<string, object>? stockPriceResponse = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            return stockPriceResponse;
        }

        /// <summary>
        /// Get all stock list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            List<Dictionary<string, string>>? stockListResponse = await _finnhubRepository.GetStocks();

            return stockListResponse;
        }

        /// <summary>
        /// Search stock by symbol
        /// </summary>
        /// <param name="stockSymbolToSearch"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            Dictionary<string, object>? stockResponse = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            return stockResponse;
        }
    }
}
