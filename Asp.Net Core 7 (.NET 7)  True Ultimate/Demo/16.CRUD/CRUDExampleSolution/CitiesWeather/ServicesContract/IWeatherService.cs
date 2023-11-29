using WeatherApp.Models;

namespace WeatherApp.ServiceContracts
{
    public interface IWeatherService
    {
        /// <summary>
        ///  Returns a list of CityWeather objects that contains weather details of cities
        /// </summary>
        /// <returns>List of all City Weather data of cities</returns>
        List<CityWeather> GetWeatherDetails();

        /// <summary>
        /// Returns an object of CityWeather based on the given city code
        /// </summary>
        /// <param name="CityCode">CityCode to show weather</param>
        /// <returns>return a city with weather data of specified city</returns>
        CityWeather? GetWeatherByCityCode(string CityCode);
    }
}

