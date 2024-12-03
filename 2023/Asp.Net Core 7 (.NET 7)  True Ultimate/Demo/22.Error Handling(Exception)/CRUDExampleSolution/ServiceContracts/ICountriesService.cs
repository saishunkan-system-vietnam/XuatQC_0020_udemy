using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents bussiness logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">country object to add</param>
        /// <returns>Return the country object after adding it(if success it will be return object with ID)</returns>
        CountryResponse AddCountry(CountryAddRequest countryAddRequest);

        /// <summary>
        /// Return list of all country in countries list
        /// </summary>
        /// <returns>All country in countries list</returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Return country from counties list by the country id 
        /// </summary>
        /// <param name="countryId">country id</param>
        /// <returns>country from list of countries</returns>
        CountryResponse? GetCountryById(Guid? countryId);
    }
}