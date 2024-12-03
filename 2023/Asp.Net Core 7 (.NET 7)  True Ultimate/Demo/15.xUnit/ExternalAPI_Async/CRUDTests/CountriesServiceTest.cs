using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.DTO;
using Services;
using EntityFrameworkCoreMock;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            var countriesInitialData = new List<Country>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            ApplicationDbContext dbContext = dbContextMock.Object;

            _countriesService = new CountriesService();

        }


        #region AddCountry
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
        #endregion

        #region GetAllCountries

        [Fact]
        // When just added few country, it should be return those country in list
        public void GetAllCountries_ReturnAdded_Countries()
        {
            // Arrange
            CountryAddRequest addRequest = new CountryAddRequest() { CountryName = "Turkey" };
            CountryAddRequest addRequest1 = new CountryAddRequest() { CountryName = "Japan" };

            // Act
            List<CountryResponse> countriesAddResponse = new List<CountryResponse>();
            countriesAddResponse.Add(_countriesService.AddCountry(addRequest));
            countriesAddResponse.Add(_countriesService.AddCountry(addRequest1));

            // Get all countries
            List<CountryResponse> actualCountriesResponse = _countriesService.GetAllCountries();

            // Assert
            foreach (var expectedCountry in countriesAddResponse)
            {
                Assert.True(expectedCountry.CountryID != Guid.Empty);
                Assert.Contains(expectedCountry, actualCountriesResponse);
            }
        }
        #endregion


        #region GetCountryByCountryID_ValidCountryID
        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            // Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest() { CountryName = "America" };
            CountryResponse countryResponse_add = _countriesService.AddCountry(countryAddRequest);

            // Act
            CountryResponse? countryResponse_get = _countriesService.GetCountryById(countryResponse_add.CountryID);

            // Assert
            Assert.Equal(countryResponse_add, countryResponse_get);
        }

        #endregion
    }
}
