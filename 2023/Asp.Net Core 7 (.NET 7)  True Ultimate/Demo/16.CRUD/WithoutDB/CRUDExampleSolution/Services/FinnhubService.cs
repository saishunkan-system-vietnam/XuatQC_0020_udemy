using Newtonsoft.Json;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private const string _finhubApiBaseURL = "https://finnhub.io/api/v1/";
        private const string _apiKey = "cc676uaad3i9rj8tb1s0";

        public FinnhubService()
        {
        }

        /// <summary>
        /// Get list profile of the company  
        /// </summary>
        /// <param name="stockSymbol">stock symbol</param>
        /// <returns>return the list profile of the company</returns>
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol)) return null;
            Dictionary<string, object>? profile2List = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_finhubApiBaseURL}stock/profile2?symbol={stockSymbol}&token={_apiKey}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(apiResponse)) return null;
                    profile2List = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                }
            }

            return profile2List;
        }

        /// <summary>
        /// Get list of the stock price
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns>return the list of stock price</returns>
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol)) return null;
            Dictionary<string, object>? stockPriceList = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_finhubApiBaseURL}quote?symbol={stockSymbol}&token={_apiKey}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(apiResponse)) return null;
                    stockPriceList = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);
                }
            }

            return stockPriceList;
        }
    }
}
