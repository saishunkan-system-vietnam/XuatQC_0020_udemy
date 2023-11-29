using eCommerceOrder.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eCommerceOrder.CustomValidations
{
    public class PriceValidatorAttribute: ValidationAttribute
    {
        public string OtherPropertyName { get; set; }
        public PriceValidatorAttribute(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {
                double invoicePrice = Convert.ToDouble(value);


                // get PropertyInfo
                PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

                // other propertyValue
                List<Product> productList = (List<Product>)otherProperty.GetValue(validationContext.ObjectInstance);

                if (productList == null || productList.Count == 0)
                {
                    return null;
                }
                double totalPrice = 0;
                foreach (var product in productList)
                {
                    totalPrice += product.Price * product.Quantity;
                }

                if (totalPrice != invoicePrice)
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
