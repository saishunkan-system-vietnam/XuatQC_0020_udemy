using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {

        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubService> _logger;

        public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get list profile of the company  
        /// </summary>
        /// <param name="stockSymbol">stock symbol</param>
        /// <returns>return the list profile of the company</returns>
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("GetCompanyProfile method of FinnhubService");
            _logger.LogDebug("GetCompanyProfile Request parameter: {stockSymbol}", stockSymbol);

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
            _logger.LogInformation("GetStockPriceQuote  method of FinnhubService");
            _logger.LogDebug("GetStockPriceQuote Request parameter: {stockSymbol}", stockSymbol);
            Dictionary<string, object>? stockPriceResponse = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            return stockPriceResponse;
        }

        /// <summary>
        /// Get all stock list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            _logger.LogInformation("GetStocks method of FinnhubService");
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
            _logger.LogInformation("GetStocks method of FinnhubService");
            _logger.LogDebug("SearchStocks Request parameter: {stockSymbolToSearch}", stockSymbolToSearch);
            Dictionary<string, object>? stockResponse = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            return stockResponse;
        }
    }
}
