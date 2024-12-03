using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represent business logic for manulating Person Entity
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Add a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>Returns the same person details, with personID</returns>
        PersonResponse AddPerson(PersonAddRequest personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Return a list of objects of personResponse</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Return person from the person list
        /// </summary>
        /// <param name="personId">person id to get</param>
        /// <returns>personResponse from the person list</returns>
        PersonResponse? GetPersonById(Guid? personId);

        /// <summary>
        /// Return all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Return all matching persons based on the given search field and search string</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Return sorted list of persons
        /// </summary>
        /// <param name="allPersons">List of person to sort</param>
        /// <param name="sortBy">Name of the property(key), base on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>List of persons after sorted</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortedOrderOptions sortOrder);

        /// <summary>
        /// Return person after updated
        /// </summary>
        /// <param name="personUpdateRequest">person parameter to update</param>
        /// <returns>Person response</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Delete a person from the persons list
        /// </summary>
        /// <param name="personID">Person id to delete</param>
        /// <returns>true if delete success, fail if delete fail</returns>
        bool DetletePerson(Guid personID);
    }
}
