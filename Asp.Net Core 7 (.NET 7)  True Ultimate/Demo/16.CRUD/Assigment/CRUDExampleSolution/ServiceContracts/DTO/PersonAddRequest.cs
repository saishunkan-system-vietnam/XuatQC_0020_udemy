using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Peson name can not be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can not be blank")]
        [EmailAddress(ErrorMessage = "Email address should be valid value")]
        [Remote(action: "IsExistedEmail", controller: "Persons")] // remote attribute handle validation by specigy action(validation action method) and controller to
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReciveNewsLetters { get; set; }
        public Person ToPerson()
        {
            return new Person { PersonName = PersonName, Email = Email,
                DateOfBirth = DateOfBirth, Gender = Gender.ToString(), 
                CountryID = CountryID, Address = Address, ReciveNewsLetters = ReciveNewsLetters }; 
        }
    }
}
