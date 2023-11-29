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
    }
}