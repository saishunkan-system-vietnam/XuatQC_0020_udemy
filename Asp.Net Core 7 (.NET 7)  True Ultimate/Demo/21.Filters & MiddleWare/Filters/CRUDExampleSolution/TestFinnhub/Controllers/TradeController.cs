﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using TestFinnhub.Models;
using TradeOrders.Controllers;
using TradeOrders.Filters;

namespace TestFinnhub.Controllers
{
    [Route("trade")]
    public class TradeController : BaseController
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _stocksService = stocksService; 
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            string stockName = _tradingOptions.Value.DefaultStockSymbol;
            var stockInfor = await _finnhubService.GetCompanyProfile(stockName);
            var stockPrice = await _finnhubService.GetStockPriceQuote(stockName);

            StockTrade stockTrade = new StockTrade();
            if (stockInfor != null && stockPrice != null)
            {
                stockTrade.StockName = stockInfor["name"].ToString();
                stockTrade.StockSymbol = stockInfor["ticker"].ToString();
                stockTrade.Price = Convert.ToDouble(stockPrice["c"].ToString());
            }

            @ViewBag.FinnhubToken = "xxx";

            return View(stockTrade);
        }

        [Route("buy-order")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest? orderRequest)
        {
            // It initializes "DateAndTimeOfOrder"
            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(orderRequest);


            //  in case of validation errors in the model object, it reinvokes the same view, along with same model object.
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() 
                { StockName = orderRequest.StockName, 
                  Quantity = orderRequest.Quantity, 
                  StockSymbol = orderRequest.StockSymbol 
                };
                return View("Index", stockTrade);
            }
            // If model state has no errors, it invokes StocksService.CreateBuyOrder()
            await _stocksService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("sell-order")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(orderRequest);

            //  in case of validation errors in the model object, it reinvokes the same view, along with same model object.
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade()
                {
                    StockName = orderRequest.StockName,
                    Quantity = orderRequest.Quantity,
                    StockSymbol = orderRequest.StockSymbol
                };
                return View("Index", stockTrade);
            }

            await _stocksService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }


        [Route("oders")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);
        }

        [Route("orders-pdf")]
        public async Task<IActionResult> OrdersPDF()
        {
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