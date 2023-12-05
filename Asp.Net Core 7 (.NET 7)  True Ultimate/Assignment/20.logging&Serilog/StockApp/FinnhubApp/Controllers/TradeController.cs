using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using TestFinnhub.Models;

namespace TestFinnhub.Controllers
{
    [Route("trade")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;
        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, 
            IStocksService stocksService, IConfiguration configuration, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _stocksService = stocksService;
            _configuration = configuration;
            _logger = logger;
        }

        [Route("index/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            _logger.LogInformation("Index action method of TradeController");
            _logger.LogDebug("Index Request parameter: {stockSymbol}", stockSymbol);

            string stockSym = !string.IsNullOrEmpty(stockSymbol)? stockSymbol : _tradingOptions.Value.DefaultStockSymbol;
            var stockInfor = await _finnhubService.GetCompanyProfile(stockSym);
            var stockPrice = await _finnhubService.GetStockPriceQuote(stockSym);

            StockTrade stockTrade = new StockTrade();
            if (stockInfor != null && stockPrice != null)
            {
                stockTrade.StockName = stockInfor["name"].ToString();
                stockTrade.StockSymbol = stockInfor["ticker"].ToString();
                stockTrade.Price = Convert.ToDouble(stockPrice["c"].ToString());
            }

            @ViewBag.FinnhubToken = _configuration["FinnhubApiToken"];

            return View(stockTrade);
        }

        [Route("buy-order")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            _logger.LogInformation("BuyOrder action method of TradeController");
            _logger.LogDebug("BuyOrder Request parameter: {buyOrderRequest}", buyOrderRequest);
            // It initializes "DateAndTimeOfOrder"
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);


            //  in case of validation errors in the model object, it reinvokes the same view, along with same model object.
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() 
                { StockName = buyOrderRequest.StockName, 
                  Quantity = buyOrderRequest.Quantity, 
                  StockSymbol = buyOrderRequest.StockSymbol 
                };
                return View("Index", stockTrade);
            }
            // If model state has no errors, it invokes StocksService.CreateBuyOrder()
            await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("sell-order")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            _logger.LogInformation("SellOrder action method of TradeController");
            _logger.LogDebug("SellOrder Request parameter: {SellOrderRequest}", sellOrderRequest);
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            //  in case of validation errors in the model object, it reinvokes the same view, along with same model object.
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade()
                {
                    StockName = sellOrderRequest.StockName,
                    Quantity = sellOrderRequest.Quantity,
                    StockSymbol = sellOrderRequest.StockSymbol
                };
                return View("Index", stockTrade);
            }

            await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }


        [Route("oders")]
        public async Task<IActionResult> Orders()
        {
            _logger.LogInformation("Orders action method of TradeController");
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);
        }

        [Route("orders-pdf")]
        public async Task<IActionResult> OrdersPDF()
        {
            _logger.LogInformation("OrdersPDF action method of TradeController");
            List<TradeOrderResponse> tradeOrderResponses = await _stocksService.GetTradeOrders();


            return new ViewAsPdf("OrdersPDF", tradeOrderResponses, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Bottom = 20,
                    Left = 20,
                    Right = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

    }
}