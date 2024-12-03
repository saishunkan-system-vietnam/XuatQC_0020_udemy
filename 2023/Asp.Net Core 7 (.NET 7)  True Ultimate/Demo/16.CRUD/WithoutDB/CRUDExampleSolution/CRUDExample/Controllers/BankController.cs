using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    [Route("bank")]
    public class BankController : Controller
    {
        private readonly dynamic _bank_accouts;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BankController(IHttpContextAccessor httpContextAccessor)
        {
            _bank_accouts = new
            {
                accountNumber = 1001,
                accountHolderName = "Example Name",
                currentBalance = 5000
            };

            _httpContextAccessor = httpContextAccessor;
        }

        // Example #1:
        [Route("/")]
        public ContentResult Index()
        {
            Response.StatusCode = 200;

            var welcomeMessage = "Welcome to the Best Bank";
            return Content(welcomeMessage, "text/plain");
        }

        // Example #2:
        [Route("account-details")]
        public JsonResult Details()
        {
            Response.StatusCode = 200;
            return Json(_bank_accouts);
        }

        // GET: BankController/Create
        //[Route("account-statement")]
        //public FileResult downloadDummyFile()
        //{
        //    Response.StatusCode = 200;
        //    var filePath = "bankAccount.pdf"; // for dummy file
        //    return File(filePath, "application/pdf");
        //}

        // Example #3:
        [Route("account-statement")]
        public IActionResult downloadDummyFile()
        {
            Response.StatusCode = 200;
            return Content("[some dummy PDF file]", "text/plain");
        }

        // Example #4, #5:
        [Route("get-current-balance/{accountNumber}")]
        [Route("get-current-balance")]
        public IActionResult GetBalance(int? accountNumber)
        {
            var localIp = _httpContextAccessor.HttpContext.Connection.LocalIpAddress;
            var remoteIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

            if (accountNumber == 0 || accountNumber == null)
            {
                return NotFound("Account Number should be supplied.");
            }

            Response.StatusCode = 200;

            if (accountNumber == _bank_accouts.accountNumber)
            {
                return Content(_bank_accouts.currentBalance.ToString(), "text/plain");
            }
            else
            {
                return BadRequest($"Account Number should be {_bank_accouts.accountNumber}");
            }

            
        }
    }
}
