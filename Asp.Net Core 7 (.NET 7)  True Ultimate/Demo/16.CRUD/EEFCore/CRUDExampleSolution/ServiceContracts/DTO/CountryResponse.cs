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

        public override bool Equals(object? country2Compare)
        {
            if (country2Compare == null) return false;

            if (country2Compare.GetType() != typeof(CountryResponse)) return false;

            CountryResponse countryParam = (CountryResponse)country2Compare;

            return this.CountryID == countryParam.CountryID &&
                this.CountryName == countryParam.CountryName;
        }
    }

    public static class CountyExtension
    {
        public static CountryResponse ToCountryRespone(this Country country)
        {
            return new CountryResponse { CountryID = country.CountryID, CountryName = country.CountryName };
        }
    }
}
