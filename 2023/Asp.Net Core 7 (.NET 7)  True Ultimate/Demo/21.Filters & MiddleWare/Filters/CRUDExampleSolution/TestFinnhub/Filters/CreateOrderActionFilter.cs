using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using TestFinnhub.Controllers;
using TestFinnhub.Models;

namespace TradeOrders.Filters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;

                if (orderRequest != null)
                {
                    //update date of order
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    //re-validate the model object after updating the date
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };

                        context.Result = tradeController.View(nameof(TradeController.Index), stockTrade); //skips the subsequent action filters & action method

                    }
                    else
                    {
                        //invokes the subsequent filter or action method
                        await next();
                    }
                }
                else
                {
                    //invokes the subsequent filter or action method
                    await next();
                }
            }
            else
            {
                //invokes the subsequent filter or action method
                await next();
            }
        }
    }
}