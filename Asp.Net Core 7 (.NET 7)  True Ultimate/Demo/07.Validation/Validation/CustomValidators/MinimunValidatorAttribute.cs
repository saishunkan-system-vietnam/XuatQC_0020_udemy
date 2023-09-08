using System.ComponentModel.DataAnnotations;

namespace Validation.CustomValidators
{
    public class MinimunValidatorAttribute: ValidationAttribute
    {
        public int MinimunYear { get; set; } = 2000;
        public string DefaultMesssage { get; set; } = "Year is should greater than {0}";

        public MinimunValidatorAttribute() { }

        public MinimunValidatorAttribute(int minimunYear) 
        { 
            MinimunYear = minimunYear;
        }   

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null) 
            {
                DateTime date = (DateTime)value;
                if (date.Year < MinimunYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultMesssage, MinimunYear));
                }
            }
            else
            {
                return ValidationResult.Success;
            }
            return null;
        }
    }
}
