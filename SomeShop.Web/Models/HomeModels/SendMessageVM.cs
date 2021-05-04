using System.ComponentModel.DataAnnotations;

namespace SomeShop.Web.Models.HomeModels
{
    public class SendMessageVM
    {
        [Required(ErrorMessage = "Your name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Your email is required!")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Your message is required!")]
        public string Message { get; set; }
        public bool IsNotRobot { get; set; }
    }
}
