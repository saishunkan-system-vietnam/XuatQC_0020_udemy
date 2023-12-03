using Microsoft.AspNetCore.Mvc;

namespace Configuration.Controllers
{
    public class TradeController : Controller
    {
        [Route("trade/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
