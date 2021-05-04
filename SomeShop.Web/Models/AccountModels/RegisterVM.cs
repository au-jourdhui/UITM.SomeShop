using System.ComponentModel.DataAnnotations;

namespace SomeShop.Web.Models.AccountModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Wrong email format!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} should be of length {2} characters.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain 3 of the 4 following characters: upper case (A-Z), lower case (a-z), number (0-9), and special character (for example! @ # $% ^ & *)")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required!")]
        [Display(Name = "Confirm password")]
        public string RepeatPassword { get; set; }
    }
}
