using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment; 

        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FileName}.{Method}\n{ExceptionType}\n {ExceptionMessage}",
                nameof(ExceptionHandlingFilter), nameof(OnException), context.Exception.GetType().ToString(), 
                context.Exception.Message);

            if (_environment.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
            }
        }
    }
}
