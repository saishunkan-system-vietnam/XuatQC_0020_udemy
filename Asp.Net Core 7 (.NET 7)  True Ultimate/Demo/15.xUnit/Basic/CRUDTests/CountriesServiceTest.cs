using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest() => _countriesService = new CountriesService();

        // When CountryAddRequest is null, it should be throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            // Arrange 
            CountryAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When the countryName is null, it shoud throw ArgumentException
        [Fact]
        public void AddCountry_NullCountryName()
        {
            // Arrange
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = null
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When the countryName is dupplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DupplicateCountryName()
        {
            // Arrange
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = "VN"
            };

            CountryAddRequest request1 = new CountryAddRequest()
            {
                CountryName = "VN"
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
                _countriesService.AddCountry(request1);
            });
        }

        // When you supply proper country name, it should isert(add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            // Act
            CountryResponse response = _countriesService.AddCountry(request);

            // Assert
            Assert.True(response.CountryID != Guid.Empty);
        }
    }
}
