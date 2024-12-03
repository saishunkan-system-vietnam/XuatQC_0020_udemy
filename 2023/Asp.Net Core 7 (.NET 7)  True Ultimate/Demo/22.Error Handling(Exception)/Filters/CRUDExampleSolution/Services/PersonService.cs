using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {
            // Check if personAddRequest is not null
            if (personAddRequest == null) { throw new ArgumentNullException(nameof(personAddRequest)); }

            // check if personName is not null
            if (string.IsNullOrEmpty(personAddRequest.PersonName)) { throw new ArgumentException("Person name cannot be blank"); }

            // Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            Person personRequest = personAddRequest.ToPerson();
            Person personResponse = _personRepository.AddPerson(personRequest);

            // return personResonse
            return personResponse.ToPersonResponse();
        }

        public List<PersonResponse> GetAllPersons()
        {
            List<PersonResponse> personresponses = new List<PersonResponse>();

            // using Eager loading
            var persons = _personRepository.GetAllPersons();
            foreach (var person in persons)
            {
                personresponses.Add(person.ToPersonResponse());
            }
            return personresponses;
        }

        public PersonResponse? GetPersonById(Guid? personId)
        {
            if (personId == null) { return null; }

            // using Eager loading
            Person? person = _personRepository.GetPersonById(personId);

            if (person == null) { return null; }

            return person.ToPersonResponse();
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
                    (!string.IsNullOrEmpty(temp.CountryName) ?
                    temp.CountryName.Contains(searchString,
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
                (nameof(PersonResponse.CountryName), SortedOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.CountryName.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.CountryName), SortedOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.CountryName.ToString(), StringComparer.OrdinalIgnoreCase).ToList(),

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

            // if person name is null or empty, it should throw argument exception
            if (string.IsNullOrEmpty(personUpdateRequest.PersonName)) { throw new ArgumentException("PersonName can not be null or blank"); }

            Person personRequest = personUpdateRequest.ToPerson();
            Person personResponse = _personRepository.UpdatePerson(personRequest);

            if (personResponse == null) { throw new InvalidPersonIDException("PersonID is not valid"); }

            return personResponse.ToPersonResponse();
        }

        public bool DetletePerson(Guid personID)
        {
            return _personRepository.DetletePerson(personID);
        }

        public bool IsRegistedMail(string email, Guid personId)
        {
            return _personRepository.IsRegistedMail(email, personId);
        }
    }
}
