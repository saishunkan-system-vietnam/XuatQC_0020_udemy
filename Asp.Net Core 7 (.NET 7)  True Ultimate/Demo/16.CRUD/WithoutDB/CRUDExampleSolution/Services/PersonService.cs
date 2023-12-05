using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;
using System.Diagnostics;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _personList;
        private readonly ICountriesService _countryService;
        public PersonService(ICountriesService countriesService, bool initialize = true)
        {
            _personList = new List<Person>();
            _countryService = countriesService;
            if (initialize)
            {
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("580341A8-A9C9-4D75-B5B4-2F4F4DCCC774"),
                    PersonName = "Kellen",
                    Email = "kretchford0@cam.ac.uk",
                    DateOfBirth = Convert.ToDateTime("2000-08-03"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "59 Lien Center",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("A023EDE4-FD63-4805-844B-6802C6AAEB58")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("01A0E031-7936-44DC-B883-63349642A4CD"),
                    PersonName = "Windy",
                    Email = "wmaccallum1@naver.com",
                    DateOfBirth = Convert.ToDateTime("2000-04-25"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "68 Johnson Trail",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("538CECD4-9B4A-448C-AA99-61F7E9CF03A2")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("917D153D-FA29-47E3-9387-FDDDBA49B33B"),
                    PersonName = "Murry",
                    Email = "msand2@abc.net.au",
                    DateOfBirth = Convert.ToDateTime("2000-08-30"),
                    Gender = GenderOptions.Male.ToString(),
                    Address = "93 Algoma Circle",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("BF18034C-32BE-443D-A937-3E02F9C6DD71")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("E2427F2E-8DE1-41EE-9354-1EDF727A8C8C"),
                    PersonName = "Alexa",
                    Email = "amcfadin3@skype.com",
                    DateOfBirth = Convert.ToDateTime("2000-06-26"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "67488 Southridge Way",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("2491D408-49A0-4151-8C26-AB40C84712D5")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("6E4C7B35-2836-4937-B8BD-71D69B8535AB"),
                    PersonName = "Percival",
                    Email = "pmasterman4@webmd.com",
                    DateOfBirth = Convert.ToDateTime("2000-09-01"),
                    Gender = GenderOptions.Male.ToString(),
                    Address = "6017 American Plaza",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("33AE1C66-6553-4408-AC18-533AAE2D2E75")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("A274A654-ACEF-4E95-B207-48809AA6A19A"),
                    PersonName = "Breanne",
                    Email = "bdevere5@dedecms.com",
                    DateOfBirth = Convert.ToDateTime("2000-09-03"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "50844 Starling Pass",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("BB1CD046-38AA-4902-9793-49B8AB5E144F")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("A58AC937-0BE4-4B15-A533-14FD68101922"),
                    PersonName = "Wolfgang",
                    Email = "wtysall6@dyndns.org",
                    DateOfBirth = Convert.ToDateTime("2000-03-26"),
                    Gender = GenderOptions.Male.ToString(),
                    Address = "890 Hanson Terrace",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("A023EDE4-FD63-4805-844B-6802C6AAEB58")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("1A9AE4BE-0BA4-4FA6-936D-1B50E6E91657"),
                    PersonName = "Hershel",
                    Email = "hmckeurton7@ca.gov",
                    DateOfBirth = Convert.ToDateTime("2000-01-26"),
                    Gender = GenderOptions.Male.ToString(),
                    Address = "2703 Kennedy Hill",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("A023EDE4-FD63-4805-844B-6802C6AAEB58")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("4541AC9B-C772-4C4B-AD2F-9B71B3C1BFB2"),
                    PersonName = "Donni",
                    Email = "dyarker8@t-online.de",
                    DateOfBirth = Convert.ToDateTime("2000-03-21"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "0822 Tennessee Road",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("A023EDE4-FD63-4805-844B-6802C6AAEB58")
                });
                _personList.Add(new Person
                {
                    PersonID = Guid.Parse("3CCEDB72-7D09-4F7A-A79A-37554063B2DA"),
                    PersonName = "Diena",
                    Email = "dboyton9@deviantart.com",
                    DateOfBirth = Convert.ToDateTime("2000-10-24"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "75433 Westport Terrace",
                    ReciveNewsLetters = true,
                    CountryID = Guid.Parse("A023EDE4-FD63-4805-844B-6802C6AAEB58")
                });
            }
        }

        private PersonResponse ConvertToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countryService.GetCountryById(person.CountryID)?.CountryName;

            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {
            // Check if personAddRequest is not null
            if (personAddRequest == null) { throw new ArgumentNullException(nameof(personAddRequest)); }

            // check if personName is not null
            if (string.IsNullOrEmpty(personAddRequest.PersonName)) { throw new ArgumentException("Person name cannot be blank"); }

            // Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            // change to person to generate person type
            Person person = personAddRequest.ToPerson();

            // generate new person ID
            person.PersonID = new Guid();

            // add person to person list
            _personList.Add(person);

            // return personResonse
            return this.ConvertToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (Person person in _personList)
            {
                personResponses.Add(this.ConvertToPersonResponse(person));
            }
            return personResponses;
        }

        public PersonResponse? GetPersonById(Guid? personId)
        {
            if (personId == null) { return null; }

            Person? person = _personList.FirstOrDefault(x => x.PersonID == personId);

            if (person == null) { return null; }

            return this.ConvertToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.PersonName) ?
                    temp.PersonName.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp =>
                   (!string.IsNullOrEmpty(temp.Email) ?
                   temp.Email.Contains(searchString,
                   StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                   (temp.DateOfBirth != null) ?
                   temp.DateOfBirth.Value.ToString("yyyy/MM/dd").Contains(searchString,
                   StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.ToLower() == searchString.ToLower() : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Age):
                    matchingPersons = allPersons.Where(temp =>
                    ((temp.Age != null) ?
                    temp.Age == Convert.ToDouble(searchString) : true)).ToList();
                    break;

                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortedOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                // PersonName
                (nameof(Person.PersonName), SortedOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(Person.PersonName), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                // Email
                (nameof(Person.Email), SortedOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(Person.Email), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                // Date of birth
                (nameof(Person.DateOfBirth), SortedOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.DateOfBirth.Value.ToString("yyyy/MM/dd"), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(Person.DateOfBirth), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.DateOfBirth.Value.ToString("yyyy/MM/dd"), StringComparer.OrdinalIgnoreCase).ToList(),

                // Address
                (nameof(Person.Address), SortedOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(Person.Address), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                // ReceiveNewLetters
                (nameof(Person.ReciveNewsLetters), SortedOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.ReciveNewsLetters.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(Person.ReciveNewsLetters), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.ReciveNewsLetters.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

                // Country
                (nameof(PersonResponse.Country), SortedOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.Country.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.Country.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

                // Gender
                (nameof(PersonResponse.Gender), SortedOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.Gender.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.Gender.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

                // Age
                (nameof(PersonResponse.Age), SortedOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Age.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Age), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.Age.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            // if person update request is null, it should throw null exception
            if (personUpdateRequest == null) { throw new ArgumentNullException(nameof(PersonUpdateRequest)); }

            // validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            // if personId is not valid, it should throw argument exception
            PersonResponse personResponse_fromGet = GetPersonById(personUpdateRequest.PersonID);
            if (personResponse_fromGet == null) { throw new ArgumentException("PersonID is not valid"); }

            // if person name is null or empty, it should throw argument exception
            if (string.IsNullOrEmpty(personUpdateRequest.PersonName)) { throw new ArgumentException("PersonName can not be null or blank"); }

            //personResponse_fromGet = GetPersonById(personUpdateRequest.PersonID);

            PersonResponse personResponse_fromUpdate = null;

            for (int i = 0; i <= _personList.Count; i++)
            {
                if (personUpdateRequest.PersonID == _personList[i].PersonID)
                {
                    _personList[i] = personUpdateRequest.ToPerson();
                    personResponse_fromUpdate = _personList[i].ToPersonResponse();
                    break;
                }
            }

            return personResponse_fromUpdate;
        }

        public bool DetletePerson(Guid personID)
        {
            PersonResponse personResponse_fromGet = GetPersonById(personID);

            if (personResponse_fromGet == null) { return false; }
            PersonUpdateRequest personUpdateRequest = personResponse_fromGet.ToPersonUpdateRequest();
            Person person = personUpdateRequest.ToPerson();

            _personList.RemoveAll(temp => temp.PersonID == person.PersonID);
            return true;
        }
    }
}
