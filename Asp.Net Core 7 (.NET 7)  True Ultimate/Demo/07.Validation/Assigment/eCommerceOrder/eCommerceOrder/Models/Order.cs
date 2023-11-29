using eCommerceOrder.CustomValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace eCommerceOrder.Models
{
    public class Order
    {
        [BindNever]
        public int? OrderNo { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty")]
        [DisplayName("Order Date")]
        [DateValidator("2000-01-01")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "{0} cannot be empty")]
        [PriceValidator("Products", ErrorMessage = "Invoice price should be equal to the total cost of all product in order")]
        [DisplayName("Invoice Price")]
        public double InvoicePrice { get; set; }

        [ProductListValidator(ErrorMessage = "{0} should be has at least 1 product")]
        public List<Product> Products { get; set; }
    }
}
