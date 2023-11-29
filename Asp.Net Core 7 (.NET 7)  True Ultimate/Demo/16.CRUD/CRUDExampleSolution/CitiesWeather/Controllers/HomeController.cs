using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using WeatherApp.ServiceContracts;

namespace WeatherApp.Controllers
{
    [Route("weather")]
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            List<CityWeather> cityWeatherList = _weatherService.GetWeatherDetails();
            return View(cityWeatherList);
        }

        [HttpGet]
        [Route("{cityCode}")]
        public IActionResult CityWeather(string cityCode)
        {
            if (string.IsNullOrEmpty(cityCode)) { return View(); }

            CityWeather? cityWeather = _weatherService.GetWeatherByCityCode(cityCode);

            return View(cityWeather);
        }
    }
}