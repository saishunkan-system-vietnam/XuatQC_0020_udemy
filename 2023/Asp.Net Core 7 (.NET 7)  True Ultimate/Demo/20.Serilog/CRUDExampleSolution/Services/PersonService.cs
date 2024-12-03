using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;
using System.Diagnostics;
using System.Threading.Channels;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly PersonDBContext _db;
        private readonly ICountriesService _countryService;
        private readonly ILogger<PersonService> _logger;
        public PersonService(PersonDBContext personDBContext, ICountriesService countriesService, ILogger<PersonService> logger)
        {
            _db = personDBContext;
            _countryService = countriesService;
            _logger = logger;
        }

        private PersonResponse ConvertToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countryService.GetCountryById(person.CountryID)?.CountryName;

            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {
            _logger.LogInformation("AddPerson of PersonService ");
            _logger.LogDebug("AddPerson of PersonService parameter {personAddRequest} ", personAddRequest);
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
            _db.Persons.Add(person);
            _db.SaveChanges();
            // return personResonse
            return this.ConvertToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            _logger.LogInformation("GetAllPersons of PersonService ");
            List<PersonResponse> personresponses = new List<PersonResponse>();
            foreach (var person in _db.Persons.ToList())
            {
               personresponses.Add(this.ConvertToPersonResponse(person));
            }
            return personresponses;

            // return _db.Persons.Select(person => ConvertToPersonResponse(person)).ToList();
        }

        public PersonResponse? GetPersonById(Guid? personId)
        {
            _logger.LogInformation("GetPersonById of PersonService ");
            _logger.LogDebug("GetPersonById of PersonService parameter {personId} ", personId);

            if (personId == null) { return null; }

            Person? person = _db.Persons.FirstOrDefault(x => x.PersonID == personId);

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
            //PersonResponse personResponse_fromGet = GetPersonById(personUpdateRequest.PersonID);
            //if (personResponse_fromGet == null) { throw new ArgumentException("PersonID is not valid"); }

            // if person name is null or empty, it should throw argument exception
            if (string.IsNullOrEmpty(personUpdateRequest.PersonName)) { throw new ArgumentException("PersonName can not be null or blank"); }

            Person personUpdate = personUpdateRequest.ToPerson();

            _db.Update(personUpdate);
            //_db.Attach(personUpdate);
            //_db.Entry(personUpdate).State = EntityState.Modified;

            _db.SaveChanges();

            return personUpdate.ToPersonResponse();
        }

        public bool DetletePerson(Guid personID)
        {
            PersonResponse personResponse_fromGet = GetPersonById(personID);

            if (personResponse_fromGet == null) { return false; }
            PersonUpdateRequest personUpdateRequest = personResponse_fromGet.ToPersonUpdateRequest();
            Person person = personUpdateRequest.ToPerson();

            _db.Persons.Remove(person);
            _db.SaveChanges();
            return true;
        }
    }
}
