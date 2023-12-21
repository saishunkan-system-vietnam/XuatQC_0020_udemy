using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents the DTO class that contains the person detail to update
    /// </summary>
    public class PersonUpdateRequest
    {
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Peson name can not be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can not be blank")]
        [EmailAddress(ErrorMessage = "Email address should be valid value")]
        [Remote(action: "IsExistedEmail", controller: "Persons", AdditionalFields=nameof(PersonID))] // remote attribute handle validation by specigy action(validation action method) and controller to
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReciveNewsLetters { get; set; }
        public Person ToPerson()
        {
            return new Person
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReciveNewsLetters = ReciveNewsLetters
            };
        }
    }
}
