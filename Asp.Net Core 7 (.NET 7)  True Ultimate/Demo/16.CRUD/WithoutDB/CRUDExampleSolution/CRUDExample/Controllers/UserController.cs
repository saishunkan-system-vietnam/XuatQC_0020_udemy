using CRUDExample.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CRUDExample.Controllers
{
    /// <summary>
    /// A controller present for remote validation demo
    /// url: localhost:port/user
    /// </summary>
    [Route("user")]
    public class UserController : Controller
    {
        /// <summary>
        /// Function that called by model property using Remote attribute
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsUsernameValid(string username)
        {
            // check validation
            bool isAvailable = ValidateUserName(username);

            if (isAvailable)
            {
                return Json(true); // return true if valid
            }

            return Json("Username is not valid."); // return error message if not valid
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new UserViewModel();
            return View(model);
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // process when all data has valid

                return RedirectToAction("Success");
            }

            return View(model);
        }

        private bool ValidateUserName(string userName)
        {
            string parttern = "^[a-zA-Z0-9]{4,10}$";

            Regex regex = new Regex(parttern);

            return regex.IsMatch(userName);

        }

        

    }
}
