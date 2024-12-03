using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonDBContext _db;
        public CountriesService(PersonDBContext personDBContext)
        {
            _db = personDBContext;
        }
        public CountryResponse AddCountry(CountryAddRequest countryAddRequest)
        {
            // check if countryAddRequest is not null
            if (countryAddRequest is null) { throw new ArgumentNullException(nameof(countryAddRequest)); }

            // validate all properties of countryAddRquest
            if (countryAddRequest.CountryName is null) { throw new ArgumentException(nameof(countryAddRequest.CountryName)); }

            if (_db.Countries.Any(country => country.CountryName == countryAddRequest.CountryName)) { throw new ArgumentException("Given country name already exists"); }

            // convert countryAddRequest to country
            Country country = countryAddRequest.ToCountry();

            // Generate new countryID
            country.CountryID = Guid.NewGuid();

            // Add it to list<country>
            _db.Countries.Add(country);
            _db.SaveChanges();
            // Return country response with generated countryID
            return country.ToCountryRespone();

        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(country => country.ToCountryRespone()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? countryId)
        {
            if (countryId == null) return null;


            //Country? country = null;
            //try
            //{
            //    foreach (var item in _db.Countries)
            //    {
            //        if (item.CountryID == countryId)
            //        {
            //            country = item;
            //            break;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
            Country? country = _db.Countries.FirstOrDefault(country => country.CountryID == countryId);

            return country.ToCountryRespone() ?? null;
        }
    }
}