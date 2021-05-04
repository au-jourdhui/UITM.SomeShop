using System.ComponentModel.DataAnnotations;

namespace SomeShop.Web.Models.ShopModels
{
    public class OrderVM
    {
        [Required(ErrorMessage = "Email - required field!")]
        [EmailAddress(ErrorMessage = "Invalid email format!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone is required field!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address is required field!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required field!")]
        public string City { get; set; }
        [Required(ErrorMessage = "District is required field!")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Delivery is required field!")]
        public int? DeliveryTypeId { get; set; }
        [Required(ErrorMessage = "Payment type is required field!")]
        public int? PaymentTypeId { get; set; }
    }
}
