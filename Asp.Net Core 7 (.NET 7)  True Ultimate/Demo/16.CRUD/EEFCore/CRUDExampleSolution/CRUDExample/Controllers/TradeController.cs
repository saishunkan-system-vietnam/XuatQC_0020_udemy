using CRUDExample.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace CRUDExample.Controllers
{
    [Route("trade")]
    public class TradeController : Controller
    {
        
        private readonly IOptions<TradingOptions> _config;
        private readonly IFinnhubService   _finnhubService;
        public TradeController(IOptions<TradingOptions> config, IFinnhubService finnhubService)
        {
            _config = config;
            _finnhubService = finnhubService;
        }


        [Route("index")]
        public async Task<IActionResult> Index()
        {
            string stockName = _config.Value.DefaultStockSymbol;
            var stockInfor = await _finnhubService.GetCompanyProfile(stockName);
            var stockPrice = await _finnhubService.GetStockPriceQuote(stockName);

            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult BuyOrder() 
        {
            return View();
        }

        public IActionResult SellOrder() 
        {
            return View();
        }
    }
}
