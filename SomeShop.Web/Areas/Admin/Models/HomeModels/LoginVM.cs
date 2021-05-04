using System.ComponentModel.DataAnnotations;

namespace SomeShop.Web.Areas.Admin.Models.HomeModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Login is required!")]
        [EmailAddress(ErrorMessage = "Wrong email format")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} should be of length {2} characters.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
