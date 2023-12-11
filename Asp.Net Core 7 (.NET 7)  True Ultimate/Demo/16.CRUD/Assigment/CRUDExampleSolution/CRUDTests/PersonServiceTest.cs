using Entities;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper, ICountriesService countriesService, IPersonService personService)
        {
            _countriesService = countriesService;
            _personService = personService;
            _testOutputHelper = testOutputHelper;
        }

        #region Add person
        // When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrage
            PersonAddRequest personAddRequest = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        // When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            // Arrage
            PersonAddRequest personAddRequest = new PersonAddRequest() { PersonName = null };
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        // When we should supply proper person details, it should insert the person into the person list;
        // and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetail()
        {
            // Act
            PersonResponse personResponse_fromAdded = AddAPerson();


            List<PersonResponse> personResponses = _personService.GetAllPersons();

            // Assert
            Assert.Contains(personResponse_fromAdded, personResponses);
        }


        // When we should supply proper person details, it should insert the person into the person list;
        // and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public void AddPerson_AddFewPersons()
        {
            // Arrange

            // Act
            List<PersonResponse> personResponseList_fromAdded = AddMultiPerson();

            // print person response list from add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            List<PersonResponse> personResponseList_fromGet = _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_fromGet)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                Assert.Contains(personResponse_fromAdded, personResponseList_fromGet);
            }
        }

        #endregion

        #region GetFilteredPerson
        /// <summary>
        /// First we will add few persons then search base on person name with some search string. It should return the matching person
        /// </summary>
        [Fact]
        public void GetFilteredPerson_EmptySearchText()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            List<PersonResponse> personResponseList_fromGet = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_fromGet)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                Assert.Contains(personResponse_fromAdded, personResponseList_fromGet);
            }
        }

        /// <summary>
        /// First we will add few persons then search base on person name with some search string. It should return the matching person
        /// </summary>
        [Fact]
        public void GetFilteredPerson_SearchByPersonName()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                if (personResponse_fromAdded.PersonName.Contains("peter", StringComparison.OrdinalIgnoreCase))
                {
                    _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
                }
            }

            List<PersonResponse> personResponseList_fromGet = _personService.GetFilteredPersons(nameof(Person.PersonName), "peter");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_fromGet)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                if (personResponse_fromAdded.PersonName != null)
                {
                    if (personResponse_fromAdded.PersonName.Contains("peter", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponse_fromAdded, personResponseList_fromGet);
                    }
                }
            }
        }
        #endregion

        #region Get person by id
        // if we supply null as personID, it should be return null as PersonResponse
        [Fact]
        public void GetPersonByID_NullPersonID()
        {
            // Arrange
            Guid? personID = null;

            // Act
            PersonResponse personResponse_From_GetByPersonID = _personService.GetPersonById(personID);

            // Assert
            Assert.Null(personResponse_From_GetByPersonID);
        }

        // if we supply proper as personID, it should be return PersonResponse 
        [Fact]
        public void GetPersonByID_ValidPersonID()
        {
            // Arrange

            // step 1. add country to get country ID
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Vietnam" };

            CountryResponse countryAddResponse = _countriesService.AddCountry(countryAddRequest);


            // step 2. add person to the list
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "personnam@gmail.com",
                Address = "Hanoi city",
                CountryID = countryAddResponse.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1990-01-01"),
                ReciveNewsLetters = true
            };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            // Act

            // step 3. get person from the list
            PersonResponse personResponseFromGet = _personService.GetPersonById(personResponseFromAdd.PersonID);

            // Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }
        #endregion

        #region GetAllPersons
        // GetAllPersons should be return an empty list by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            // Act
            List<PersonResponse> personResponses_from_get = _personService.GetAllPersons();

            // Assert
            Assert.Empty(personResponses_from_get);
        }

        // GetAllPerson should be return list of all person
        [Fact]
        public void GetAllPersons_NotEmptyList()
        {
            // Arrange 
            // Add country
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Vietnam" };

            CountryResponse countryAddResponse = _countriesService.AddCountry(countryAddRequest);

            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "Japan" };

            CountryResponse countryAddResponse1 = _countriesService.AddCountry(countryAddRequest1);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "personnam@gmail.com",
                Address = "Hanoi city",
                CountryID = countryAddResponse.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1990-01-01"),
                ReciveNewsLetters = true
            };

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Person name 1",
                Email = "personname1@gmail.com",
                Address = "Tokyo city",
                CountryID = countryAddResponse1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1991-01-01"),
                ReciveNewsLetters = true
            };

            _personService.AddPerson(personAddRequest);
            _personService.AddPerson(personAddRequest1);

            // Act 
            List<PersonResponse> personResponses_from_get = _personService.GetAllPersons();

            // Assert
            Assert.NotEmpty(personResponses_from_get);
        }
        #endregion

        #region GetSortedPersons
        /// <summary>
        /// When we sort base on personName in ASC, it should be return person list in ascending(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_SortByName_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.PersonName).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on personName in DESC, it should be return person list in descending(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_SortByName_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.PersonName).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on Email in ASC, it should be return person list in ASC(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_SortByEmail_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.Email).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.Email), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on Email in DESC, it should be return person list in DESC(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_SortByEmail_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.Email).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.Email), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }


        /// <summary>
        /// When we sort base on DateOfBirth in ASC, it should be return person list in ASC(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_DateOfBirth_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.DateOfBirth.Value.ToString("yyyy/MM/dd")).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.DateOfBirth), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on DateOfBirth in DESC, it should be return person list in DESC(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_DateOfBirth_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.DateOfBirth.Value.ToString("yyyy/MM/dd")).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.DateOfBirth), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }


        /// <summary>
        /// When we sort base on Country in ASC, it should be return person list in ASC(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_Country_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.CountryName).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.CountryID), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on Country in DESC, it should be return person list in DESC(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_Country_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.CountryName).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.CountryID), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on Gender in ASC, it should be return person list in ASC(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_Gender_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.Gender).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.Gender), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on Gender in DESC, it should be return person list in DESC(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_Gender_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.Gender).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.Gender), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }


        /// <summary>
        /// When we sort base on Address in ASC, it should be return person list in ASC(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_Address_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.Address).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.Address), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on Address in DESC, it should be return person list in DESC(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_Address_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.Address).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.Address), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on ReceiveNewLetter in ASC, it should be return person list in ASC(smallest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_ReceiveNewLetter_ASC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = false
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = false
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderBy(temp => temp.ReciveNewsLetters).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.ReciveNewsLetters), SortedOrderOptions.ASC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }

        /// <summary>
        /// When we sort base on ReciveNewsLetter in DESC, it should be return person list in DESC(biggest value first)
        /// </summary>
        [Fact]
        public void GetSortedPersons_ReciveNewsLetter_DESC()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);

            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = false
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = false
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            // print person response list from add
            personResponseList_fromAdded = personResponseList_fromAdded.OrderByDescending(temp => temp.ReciveNewsLetters).ToList();
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponse_fromAdded in personResponseList_fromAdded)
            {
                _testOutputHelper.WriteLine(personResponse_fromAdded.ToString());
            }

            // Get all persons
            List<PersonResponse> allPersons = _personService.GetAllPersons();

            List<PersonResponse> personResponseList_toSort = _personService.GetSortedPersons(allPersons, nameof(Person.ReciveNewsLetters), SortedOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponse_fromGet in personResponseList_toSort)
            {
                _testOutputHelper.WriteLine(personResponse_fromGet.ToString());
            }

            // Assert
            for (int i = 0; i < personResponseList_fromAdded.Count; i++)
            {
                Assert.Equal(personResponseList_fromAdded[i], personResponseList_toSort[i]);
            }
        }
        #endregion

        #region Update person
        /// <summary>
        ///  When we supply null person, it should be throw ArgumentNullException
        /// </summary>
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            // Act & Asssert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        /// <summary>
        /// When we supply invalid personID, it should throw ArgumentException
        /// </summary>
        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

            // Act & Asssert
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        /// <summary>
        /// When we supply null or empty, it should throw ArgumentException
        /// </summary>
        [Fact]
        public void UpdatePerson_NullOrEmptyPersonName()
        {
            // Arrange 
            PersonResponse personResponse_fromAdd = AddAPerson();

            PersonUpdateRequest personUpdateRequest = personResponse_fromAdd.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        /// <summary>
        /// When we supply properly person detail, it should be return person reponse after update
        /// </summary>
        [Fact]
        public void UpdatePerson_PersonFullDetailUpdate()
        {
            // Arrange 
            PersonResponse personResponse_fromAdd = AddAPerson();

            PersonUpdateRequest personUpdateRequest = personResponse_fromAdd.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Name just update";

            // Act
            PersonResponse personResponse_fromUpdate = _personService.UpdatePerson(personUpdateRequest);

            // Assert
            _testOutputHelper.WriteLine("Expected: ");
            _testOutputHelper.WriteLine("Person name is: " + personUpdateRequest.PersonName);

            _testOutputHelper.WriteLine("Actual: ");
            _testOutputHelper.WriteLine("Person name is: " + personResponse_fromUpdate.PersonName);

            Assert.True(personUpdateRequest.PersonName == personResponse_fromUpdate.PersonName);
        }
        #endregion

        #region Delete person
        /// <summary>
        /// When we supply invalid person id, it should be return false
        /// </summary>
        [Fact]
        public void DeletePerson_InvalidPersonID()
        {

            // Arrange
            Guid personID = Guid.NewGuid();

            // Act
            bool isDeleted = _personService.DetletePerson(personID);

            // Assert
            Assert.False(isDeleted);
        }

        /// <summary>
        /// When we supply valid person id, it should be return true
        /// </summary>
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            // Arrange
            PersonResponse personResponse_fromAdd = AddAPerson();

            // Act
            bool isDeleted = _personService.DetletePerson(personResponse_fromAdd.PersonID);

            // Assert
            Assert.True(isDeleted);
        }
        #endregion

        private List<PersonResponse> AddMultiPerson()
        {
            // Arrange
            CountryAddRequest countryRequest = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            CountryAddRequest countryRequest1 = new CountryAddRequest()
            {
                CountryName = "ThaiLand"
            };

            CountryAddRequest countryRequest2 = new CountryAddRequest()
            {
                CountryName = "Cambodia"
            };

            CountryAddRequest countryRequest3 = new CountryAddRequest()
            {
                CountryName = "Singapore"
            };


            // Act
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryRequest2);
            CountryResponse countryResponse3 = _countriesService.AddCountry(countryRequest3);


            // Arrage
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
                new PersonAddRequest()
                {
                    PersonName = "Peterson...",
                    Email = "personnam@gmail.com",
                    Address = "Hanoi city",
                    CountryID = countryResponse.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    ReciveNewsLetters = true
                },new PersonAddRequest()
                {
                    PersonName = "Peter jordan...",
                    Email = "hue@gmail.com",
                    Address = "Hue city",
                    CountryID = countryResponse1.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1993-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Jonhson baby...",
                    Email = "danang@gmail.com",
                    Address = "Danang city",
                    CountryID = countryResponse2.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1992-01-01"),
                    ReciveNewsLetters = true
                }, new PersonAddRequest()
                {
                    PersonName = "Roger fererder...",
                    Email = "hcm@gmail.com",
                    Address = "HCM city",
                    CountryID = countryResponse3.CountryID,
                    Gender = GenderOptions.Male,
                    DateOfBirth = DateTime.Parse("1991-01-01"),
                    ReciveNewsLetters = true
                }};

            // Act
            List<PersonResponse> personResponseList_fromAdded = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);
                personResponseList_fromAdded.Add(personResponse_fromAdded);
            }

            return personResponseList_fromAdded;
        }

        private PersonResponse AddAPerson()
        {
            // Arrange
            CountryAddRequest request = new CountryAddRequest()
            {
                CountryName = "Vietnam"
            };

            // Act
            CountryResponse response = _countriesService.AddCountry(request);


            // Arrage
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name...",
                Email = "personnam@gmail.com",
                Address = "Hanoi city",
                CountryID = response.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("1990-01-01"),
                ReciveNewsLetters = true
            };

            // Act
            PersonResponse personResponse_fromAdded = _personService.AddPerson(personAddRequest);

            return personResponse_fromAdded;
        }
    }
}
