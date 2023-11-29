using eCommerceOrder.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eCommerceOrder.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost("order")]
        public IActionResult Order([FromForm][Bind] Order order) 
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }
            Random rnd = new Random();
            int randomNumber = rnd.Next(1, 99999);

            Order orderRespone = new Order();
            orderRespone = order;
            orderRespone.OrderNo = randomNumber;
            Response.StatusCode = StatusCodes.Status200OK;


            return new JsonResult(new { Data = orderRespone });

        }
    }
}
