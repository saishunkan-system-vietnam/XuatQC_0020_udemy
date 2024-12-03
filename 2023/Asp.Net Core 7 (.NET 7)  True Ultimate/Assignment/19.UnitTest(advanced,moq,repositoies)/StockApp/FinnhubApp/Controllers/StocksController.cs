﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using TestFinnhub.Models;

namespace TestFinnhub.Controllers
{
    [Route("stocks")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;

        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
        }

        [Route("/")]
        [Route("explore/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            //get company profile from API server
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary is not null)
            {
                //filter stocks
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (Top25PopularStocksList is not null)
                    {
                        stocksDictionary = stocksDictionary
                         .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }

                stocks = stocksDictionary
                 .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();
            }

            ViewBag.stock = stock;
            return View(stocks);
        }
    }
}
