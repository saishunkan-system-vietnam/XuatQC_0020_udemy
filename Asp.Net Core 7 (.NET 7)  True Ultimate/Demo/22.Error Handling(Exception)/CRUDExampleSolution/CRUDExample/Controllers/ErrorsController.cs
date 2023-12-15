using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRUDExample.Controllers
{
    public class ErrorsController : Controller
    {
        ILogger<ErrorsController> logger;
        public ErrorsController(ILogger<ErrorsController> logger)
        {
            this.logger = logger;
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>();

            logger.LogError(exceptionHandlerFeature?.Error.ToString());

            if(exceptionHandlerFeature != null && exceptionHandlerFeature.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlerFeature.Error.Message;
            }

            //return Problem(
            //    statusCode: 500,
            //    detail: "Some thing was wrong, then an error was occured, Please contact administrator to resolved that",
            //    title: "Error");

            return View();

        }
        [Route("/error-development")]
        public ActionResult DevelopmentError()
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
