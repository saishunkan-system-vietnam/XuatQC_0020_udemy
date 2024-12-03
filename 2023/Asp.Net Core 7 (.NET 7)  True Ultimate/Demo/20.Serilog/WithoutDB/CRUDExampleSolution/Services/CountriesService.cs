using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>
                {
                    new Country()
                    {
                        ID = Guid.Parse("A023EDE4-FD63-4805-844B-6802C6AAEB58"),
                        Name = "USA",
                    },
                    new Country()
                    {
                        ID = Guid.Parse("538CECD4-9B4A-448C-AA99-61F7E9CF03A2"),
                        Name = "Rusia",
                    },
                    new Country()
                    {
                        ID = Guid.Parse("BF18034C-32BE-443D-A937-3E02F9C6DD71"),
                        Name = "China",
                    },
                    new Country()
                    {
                        ID = Guid.Parse("2491D408-49A0-4151-8C26-AB40C84712D5"),
                        Name = "India",
                    },
                    new Country()
                    {
                        ID = Guid.Parse("33AE1C66-6553-4408-AC18-533AAE2D2E75"),
                        Name = "Australia",
                    },
                    new Country()
                    {
                        ID = Guid.Parse("BB1CD046-38AA-4902-9793-49B8AB5E144F"),
                        Name = "Isarel",
                    }
                });
            }
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

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryRespone()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? countryId)
        {
            if (countryId == null) return null;

            Country? country = _countries.FirstOrDefault(country => country.ID == countryId);

            return country.ToCountryRespone() ?? null;
        }
    }
}