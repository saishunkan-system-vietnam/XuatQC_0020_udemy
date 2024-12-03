using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class return type for most of countriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }

    public static class CountyExtension
    {
        public static CountryResponse ToCountryRespone(this Country country)
        {
            return new CountryResponse { CountryID = country.ID, CountryName = country.Name };
        }
    }
}
