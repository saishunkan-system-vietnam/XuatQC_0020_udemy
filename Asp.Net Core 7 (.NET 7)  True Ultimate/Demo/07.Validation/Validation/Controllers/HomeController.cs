using Microsoft.AspNetCore.Mvc;
using Validation.CustomModelsBinders;
using Validation.Models;

namespace Validation.Controllers
{
    //[ApiController]
    public class HomeController : Controller
    {
        [Route("register")]

        // for binding specify property, will pority in property attribute
        //public IActionResult Index([FromForm][Bind(nameof(Person.PersonName), nameof(Person.Phone), nameof(Person.Email))] Person person)
        // [FromBody] recive param for json and xml only
        // [FromForm] recive param from form data(form feild)
        public IActionResult Index([FromBody]Person person, [FromHeader(Name = "User-Agent")] string UserAgent) 
        {
            string userAgent = ControllerContext.HttpContext.Request.Headers["User-Agent"];
            if (!ModelState.IsValid)
            {
                List<string> lstErr = new List<string>();   
                foreach (var value in ModelState.Values)
                {
                    foreach (var err in value.Errors)
                    {
                        lstErr.Add(err.ErrorMessage);
                    }
                }
                string errors = string.Join("\n", lstErr);  
                return BadRequest(errors);
            }
            return Ok();
        }
    }
}
