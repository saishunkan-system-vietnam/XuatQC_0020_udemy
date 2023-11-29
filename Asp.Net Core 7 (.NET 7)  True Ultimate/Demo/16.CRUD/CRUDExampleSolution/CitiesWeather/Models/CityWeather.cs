namespace WeatherApp.Models
{
    public class CityWeather
    {
        public string CityUniqueCode { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public DateTime DateAndTime { get; set; }
        public int TemperatureFahrenheit { get; set; }


        public string GetBackgroundColorByTemperature(int temperature)
        {
            if (temperature < 44)
            {
                return "blue-back";
            }
            else if (temperature >= 44 && temperature < 75)
            {
                return "green-back";
            }
            else if (temperature >= 75)
            {
                return "orange-back";
            }

            return string.Empty;
        }
    }
}
