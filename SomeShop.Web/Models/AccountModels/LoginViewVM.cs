namespace SomeShop.Web.Models.AccountModels
{
    public class LoginViewVM
    {
        public LoginVM LoginVM { get; set; }
        public RegisterVM RegisterVM { get; set; }
        public string Error { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(Error);

        public LoginViewVM()
        {
            LoginVM = new LoginVM();
            RegisterVM = new RegisterVM();
        }
    }
}
