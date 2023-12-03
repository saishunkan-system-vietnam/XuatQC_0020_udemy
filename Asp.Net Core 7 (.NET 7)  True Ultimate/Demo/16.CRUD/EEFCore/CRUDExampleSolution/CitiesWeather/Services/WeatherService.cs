using WeatherApp.Models;
using WeatherApp.ServiceContracts;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly List<CityWeather> _cityWeatherList;

        public WeatherService()
        {
            _cityWeatherList = new List<CityWeather>() {
                            new CityWeather() {
                                CityUniqueCode = "LDN", CityName = "London",
                                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                                TemperatureFahrenheit = 33 },

                            new CityWeather() {
                                CityUniqueCode = "NYC",
                                CityName = "New York",
                                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                                TemperatureFahrenheit = 60 },

                            new CityWeather() {
                                CityUniqueCode = "PAR",
                                CityName = "Paris",
                                DateAndTime = Convert.ToDateTime("2030-01-01 9:00")
                                , TemperatureFahrenheit = 82 }
                           };
        }

        public List<CityWeather> GetWeatherDetails()
        {
            return _cityWeatherList;
        }

        public CityWeather? GetWeatherByCityCode(string CityCode)
        {
            CityWeather? city = _cityWeatherList.FirstOrDefault(city => city.CityUniqueCode == CityCode);
            return city;
        }
    }
}

