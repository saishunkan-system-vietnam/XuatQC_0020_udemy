using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Validation.CustomModelsBinders;
using Validation.CustomValidators;

namespace Validation.Models
{
    [ModelBinder(BinderType = typeof(PersonModelBider))]
    public class Person : IValidatableObject
    {
        [Required(ErrorMessage = "{0} cannot be null or empty")]
        [DisplayName("Person name")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} should be from {2} to {1}")]
        [RegularExpression("^[A-Za-z .]*$", ErrorMessage = "{0} should be contain alphabet, space, dot")]
        public string? PersonName { get; set; }

        [Phone(ErrorMessage = "{0} must be valid")]
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "Email not valid")]
        [BindNever]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} can not be empty")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "{0} can not be empty")]
        [Compare("Password", ErrorMessage = "{0} and {1} does not match")]
        [DisplayName("Re-enter password")]
        public string? ConfirmPassword { get; set; }

        [MinimunValidator(2006, ErrorMessage = "{0} is to small")]
        public DateTime? DateOfBirth { get; set; }

        [DisplayName("New From Date")]
        public DateTime? FromDate { get; set; }

        [DateRangeMultiValidation("FromDate", ErrorMessage = "{0} can not greater than {1}")]
        [DisplayName("New To Date")]
        public DateTime? ToDate { get; set; }

        [Range(0, 88.88, ErrorMessage = "{0} should be between from ${1} and ${2}")]
        public double? Price { get; set; }

        public int? Age { get; set; }   

        // to implement custom validate of class. Will run after all property must be valid first
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Age.HasValue == false && DateOfBirth.HasValue == false)
            {
               yield return new ValidationResult("Both of Age and Date of birth should has value");
            }
        }
    }
}
