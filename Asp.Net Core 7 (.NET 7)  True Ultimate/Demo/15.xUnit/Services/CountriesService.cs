using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest countryAddRequest)
        {
            // check if countryAddRequest is not null
            if (countryAddRequest is null) { throw new ArgumentNullException(nameof(countryAddRequest)); }

            // validate all properties of countryAddRquest
            if (countryAddRequest.CountryName is null) { throw new ArgumentException(nameof(countryAddRequest.CountryName)); }

            if (_countries.Any(country => country.Name == countryAddRequest.CountryName)) { throw new ArgumentException("Given country name already exists"); }

            // convert countryAddRequest to country
            Country country = countryAddRequest.ToCountry();

            // Generate new countryID
            country.ID = Guid.NewGuid();

            // Add it to list<country>
            _countries.Add(country);

            // Return country response with generated countryID
            return country.ToCountryRespone();

        }
    }
}