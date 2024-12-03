using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TradeOrders.Controllers;

namespace TradeOrders.Filters
{
    public class LogHandlerFilterAttribute : IActionFilter
    {
        private readonly ILogger<LogHandlerFilterAttribute> _logger;

        public LogHandlerFilterAttribute(ILogger<LogHandlerFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            string remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            string localIpAddress = context.HttpContext.Connection.LocalIpAddress?.ToString();

            _logger.LogInformation("▼------" + context.ActionDescriptor.DisplayName + " Start");
            _logger.LogInformation("User agent: " + userAgent);
            _logger.LogInformation("Remote IpAddress: " + remoteIpAddress);
            _logger.LogInformation("Local IpAddress: " + localIpAddress);

            if (context.ActionArguments.Count > 0)
            {
                _logger.LogInformation("------------------Parameter------------------");
                foreach (var arg in context.ActionArguments)
                {
                    _logger.LogInformation(arg.Key + ": " + JsonConvert.SerializeObject(arg.Value));
                }
            }

            _logger.LogInformation("------------------Respone------------------");
            _logger.LogInformation("Status: " + context.HttpContext.Response.StatusCode);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result != null)
            {
                _logger.LogInformation("Result: " + JsonConvert.SerializeObject(context.Result));
            }
            _logger.LogInformation("▲------" + context.ActionDescriptor.DisplayName + " End");
        }
    }
}
