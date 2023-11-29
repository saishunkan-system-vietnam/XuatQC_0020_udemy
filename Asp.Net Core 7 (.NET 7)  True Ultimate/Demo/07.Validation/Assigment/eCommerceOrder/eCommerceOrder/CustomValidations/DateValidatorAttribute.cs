using System.ComponentModel.DataAnnotations;

namespace eCommerceOrder.CustomValidations
{
    public class DateValidatorAttribute : ValidationAttribute
    {
        public string StrMinDate { get; set; }

        public DateValidatorAttribute() { }

        public DateValidatorAttribute(string strMinDate)
        {
            StrMinDate = strMinDate;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime orderDate = Convert.ToDateTime(value.ToString());
                DateTime mindate = Convert.ToDateTime(StrMinDate);

                if (orderDate < mindate)
                {
                    return new ValidationResult(@$"Order date should be greater than {StrMinDate}");
                }
            }

            return null;
        }
    }
}
