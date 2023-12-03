using Configuration.Models;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
           SocialMediaLinksOptions options = _configuration.GetSection("SocialMediaLinks").Get<SocialMediaLinksOptions>();
            ViewBag.FaceBook = options.Facebook;
            ViewBag.Youtube = options.Youtube;
            ViewBag.Instagram = options.Instagram;
            ViewBag.Twitter = options.Twitter;

            return View();
        }
    }
}