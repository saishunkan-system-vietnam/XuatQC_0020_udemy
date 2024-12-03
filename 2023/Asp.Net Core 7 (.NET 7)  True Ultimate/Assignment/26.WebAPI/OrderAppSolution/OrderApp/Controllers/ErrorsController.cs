using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OrderApp.Controllers
{
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        ILogger<ErrorsController> logger;
        public ErrorsController(ILogger<ErrorsController> logger)
        {
            this.logger = logger;
        }

        [Route("/error")]
        public ActionResult Error([FromServices] IHostEnvironment hostEnvironment)
        {
            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>();
            logger.LogError(exceptionHandlerFeature?.Error.ToString());

            return Problem(
                statusCode: 500,
                detail: "Some thing was wrong, then an error was occured, Please contact administrator to resolved that",
                title: "Error");

        }
        [Route("/error-development")]
        public ActionResult DevelopmentError([FromServices] IHostEnvironment hostEnvironment)
        {
            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>();
            logger.LogError(exceptionHandlerFeature?.Error.ToString());

            return Problem(
                statusCode: 500,
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);

        }
    }
}
