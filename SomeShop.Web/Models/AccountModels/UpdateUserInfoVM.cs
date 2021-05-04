using System;
using System.ComponentModel.DataAnnotations;

namespace SomeShop.Web.Models.AccountModels
{
    public class UpdateUserInfoVM
    {
        [Required(ErrorMessage = "Your name is required!")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Your email is required!")]
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
