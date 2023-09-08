using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Validation.CustomValidators
{
    public class DateRangeMultiValidationAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; set; }   
        public DateRangeMultiValidationAttribute(string otherPropertyName) 
        {
            OtherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime todate = Convert.ToDateTime(value);

                // get PropertyInfo
                PropertyInfo  otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

                // other propertyValue
                DateTime fromDate = Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance));

                if (fromDate > todate)
                {
                    var displayNameAttribute = otherProperty.GetCustomAttributes(typeof(DisplayNameAttribute), true);

                    string otherDisplayName = string.Empty;
                    if (displayNameAttribute.Length > 0)
                    {
                        DisplayNameAttribute dsp = (DisplayNameAttribute)displayNameAttribute[0];
                        otherDisplayName = dsp.DisplayName;
                    }
                    otherDisplayName = !string.IsNullOrEmpty(otherDisplayName) ? otherDisplayName : otherProperty.Name;



                    return new ValidationResult(string.Format(ErrorMessage, otherDisplayName, validationContext.DisplayName));
                }
            }
            return null;
        }
    }
}
