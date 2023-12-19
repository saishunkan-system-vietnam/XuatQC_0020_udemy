using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IPersonRepository
    {
        /// <summary>
        /// Add a new person into the list of persons
        /// </summary>
        /// <param name="person">Person to add</param>
        /// <returns>Returns the same person details, with personID</returns>
        Person AddPerson(Person person);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Return a list of objects of person</returns>
        List<Person> GetAllPersons();

        /// <summary>
        /// Get person by personId
        /// </summary>
        /// <param name="personId">person id to get</param>
        /// <returns>Return person match with parameter</returns>
        Person? GetPersonById(Guid? personId);

        /// <summary>
        /// Return person after updated
        /// </summary>
        /// <param name="person">person parameter to update</param>
        /// <returns>Person response</returns>
        Person? UpdatePerson(Person? person);

        /// <summary>
        /// Delete a person from the persons list
        /// </summary>
        /// <param name="personID">Person id to delete</param>
        /// <returns>true if delete success, fail if delete fail</returns>
        bool DetletePerson(Guid personID);

        /// <summary>
        /// Check if existed mail from user request
        /// </summary>
        /// <param name="email">email address</param>
        /// <returns>true if existed, false if not existed</returns>
        bool IsRegistedMail(string email);
    }
}
