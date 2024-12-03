using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryContracts;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private const string _finhubApiBaseURL = "https://finnhub.io/api/v1/";

        private readonly IConfiguration _configuration;

        private readonly ILogger<FinnhubRepository> _logger;
        public FinnhubRepository(IConfiguration configuration, ILogger<FinnhubRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("GetCompanyProfile method of FinnhubRepository");
            _logger.LogDebug("GetCompanyProfile Request parameter: {stockSymbol}", stockSymbol);


            if (string.IsNullOrEmpty(stockSymbol)) return null;
            Dictionary<string, object>? profile2List = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_finhubApiBaseURL}stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubApiToken"]}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(apiResponse)) return null;
                    profile2List = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                }
            }

            return profile2List;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            _logger.LogInformation("GetStockPriceQuote method of FinnhubRepository");
            _logger.LogDebug("GetStockPriceQuote Request parameter: {stockSymbol}", stockSymbol);

            if (string.IsNullOrEmpty(stockSymbol)) return null;
            Dictionary<string, object>? stockPriceList = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_finhubApiBaseURL}quote?symbol={stockSymbol}&token={_configuration["FinnhubApiToken"]}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(apiResponse)) return null;
                    stockPriceList = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                }
            }

            return stockPriceList;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            List<Dictionary<string, string>>? allStocks = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_finhubApiBaseURL}stock/symbol?exchange=US&token={_configuration["FinnhubApiToken"]}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(apiResponse)) return null;
                    allStocks = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(apiResponse);
                }
            }

            if (allStocks == null)
                throw new InvalidOperationException("Can not get data from api service");

            return allStocks;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            Dictionary<string, object>? stock = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_finhubApiBaseURL}search?q={stockSymbolToSearch}&token={_configuration["FinnhubApiToken"]}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(apiResponse)) return null;
                    stock = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                }
            }

            if (stock == null)
                throw new InvalidOperationException("Can not get data from api service");

            return stock;
        }
    }
}