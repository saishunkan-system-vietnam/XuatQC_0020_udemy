using eCommerceOrder.Models;
using System.ComponentModel.DataAnnotations;

namespace eCommerceOrder.CustomValidations
{
    public class ProductListValidatorAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string displayName = validationContext.DisplayName ?? validationContext.MemberName;
            if (value == null)
            {
                return new ValidationResult(string.Format(ErrorMessage, displayName));
            }
            List<Product> productList = (List<Product>)value;
            if (productList.Count == 0)
            {
                return new ValidationResult(string.Format(ErrorMessage, displayName));
            }

            return null;
        }
    }
}
