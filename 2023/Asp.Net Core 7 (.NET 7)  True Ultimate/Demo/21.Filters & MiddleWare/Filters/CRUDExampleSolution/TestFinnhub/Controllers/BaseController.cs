using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TradeOrders.Filters;

namespace TradeOrders.Controllers
{
    [TypeFilter(typeof(LogHandlerFilterAttribute))]
    public class BaseController : Controller
    {

    }
}
