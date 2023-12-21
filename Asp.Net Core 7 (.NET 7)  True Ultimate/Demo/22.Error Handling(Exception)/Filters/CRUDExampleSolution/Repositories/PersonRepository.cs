using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonDBContext _db;
        public PersonRepository(PersonDBContext personDBContext)
        {
            _db = personDBContext;
        }

        public Person AddPerson(Person person)
        {
            // generate new person ID
            person.PersonID = new Guid();

            // add person to person list
            _db.Persons.Add(person);
            _db.SaveChanges();

            return person;
        }

        public bool DetletePerson(Guid personID)
        {
            var person = _db.Set<Person>().FirstOrDefault(x => x.PersonID == personID);

            if (person == null) { return false; }

            _db.Persons.Remove(person);
            _db.SaveChanges();
            return true;
        }

        public List<Person> GetAllPersons()
        {
           return  _db.Persons.Include(nameof(Country)).ToList();
        }

        public Person? GetPersonById(Guid? personId)
        {
            return _db.Persons.Include(nameof(Country)).FirstOrDefault(x => x.PersonID == personId);
        }

        public bool IsRegistedMail(string email, Guid personId)
        {
            // case update
            if (personId != Guid.Empty)
            {
                var personUpdate = _db.Persons.FirstOrDefault(x => x.PersonID == personId && x.Email == email);
                if(personUpdate != null) { return false; }  // case that email belong to register person(match with ID)
            }

            // case register or other person register already exist
            var person = _db.Persons.FirstOrDefault(x => x.Email == email);

            if (person == null) { return false; }
            return true;
        }

        public Person? UpdatePerson(Person? personUpdateRequest)
        {
            // if personId is not valid, it should throw argument exception
            var existingEntity = _db.Set<Person>().Include("Country").FirstOrDefault(x => x.PersonID == personUpdateRequest.PersonID);
            if (existingEntity == null) { return null; }

            _db.Entry(existingEntity).CurrentValues.SetValues(personUpdateRequest);
            _db.SaveChanges();

            return personUpdateRequest;
        }
    }
}
