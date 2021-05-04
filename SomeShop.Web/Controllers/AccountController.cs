using SomeShop.DAL.Models;
using SomeShop.Web.Extensions;
using SomeShop.Web.Models.AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SomeShop.Web.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index(AccountTab tab = AccountTab.Dashboard)
        {
            return Index(null, tab: tab);
        }

        private IActionResult Index(string error = null, bool updatedUserInfo = false, AccountTab tab = AccountTab.Dashboard)
        {
            var model = new IndexVM
            {
                User = SystemUser,
                Error = error,
                UpdatedUserInfo = updatedUserInfo,
                Tab = tab,
                Products = UnitOfWork.StoredProcedures.GetFavouriteProductInfo(SystemUser.Id),
                OrderDetails = UnitOfWork.StoredProcedures.GetOrderDetails(SystemUser.Id)
            };

            return View("Index", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(LoginViewVM model = null)
        {
            if (model == null)
            {
                model = new LoginViewVM();
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public new IActionResult Logout()
        {
            base.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignIn(LoginVM model)
        {
            var password = UnitOfWork.StoredProcedures.MD5HashPassword(model.Password);
            if (UnitOfWork.Users.FirstOrDefault(x => x.Password == password && x.Email == model.Username) is DAL.Models.User user)
            {
                SetAuth(AuthKey, user.Token.ToString());
                return RedirectToAction("Index");
            }
            return View("Login", new LoginViewVM { LoginVM = model, Error = "Wrong login or password!" });
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.RepeatPassword)
                {
                    return View("Login", new LoginViewVM { RegisterVM = model, Error = "Password doesn't match!" });
                }
                else if (UnitOfWork.Users.FirstOrDefault(x => x.Email == model.Username) is DAL.Models.User user)
                {
                    return View("Login", new LoginViewVM { RegisterVM = model, Error = "This user is already registered in the system!" });
                }
                else
                {
                    var newUser = new DAL.Models.User
                    {
                        Email = model.Username,
                        Password = UnitOfWork.StoredProcedures.MD5HashPassword(model.Password),
                        FirstName = model.FirstName,
                        RoleId = UnitOfWork.StoredProcedures.GetUserRoleId(UserRole.User.GetDescription())
                    };
                    UnitOfWork.Users.Insert(newUser);
                    SetAuth(AuthKey, newUser.Token.ToString());
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var htmlError = ModelState.GetHtmlErrors();
                return View("Login", new LoginViewVM { RegisterVM = model, Error = htmlError });
            }
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(UpdateUserInfoVM model)
        {
            if (ModelState.IsValid)
            {
                SystemUser.Birthdate = model.Birthdate;
                SystemUser.Email = model.Email;
                SystemUser.FirstName = model.FirstName;
                SystemUser.LastName = model.LastName;
                SystemUser.Phone = model.Phone;

                var updatePassword = !string.IsNullOrWhiteSpace(model.NewPassword)
                    && (!string.IsNullOrWhiteSpace(model.ConfirmPassword)
                    || !string.IsNullOrWhiteSpace(model.CurrentPassword));

                if (updatePassword
                    && SystemUser.Password != UnitOfWork.StoredProcedures.MD5HashPassword(model.CurrentPassword))
                {
                    return Index("The current password is incorrect!");
                }
                else if (updatePassword && model.NewPassword != model.ConfirmPassword)
                {
                    return Index("The new password does not match the new one!");
                }
                else if (updatePassword
                        && SystemUser.Password == UnitOfWork.StoredProcedures.MD5HashPassword(model.CurrentPassword)
                        && model.NewPassword == model.ConfirmPassword)
                {
                    UnitOfWork.Users.UpdateUserPassword(SystemUser.Id, model.NewPassword);
                }

                UnitOfWork.Users.Update(SystemUser);
                Login(SystemUser.Token.ToString());

                return Index(updatedUserInfo: true);
            }
            else
            {
                var htmlError = ModelState.GetHtmlErrors();
                return Index(htmlError);
            }
        }

        [HttpPost]
        public IActionResult Wishlist(int productId)
        {
            var success = true;
            var favouriteProduct = (FavouriteProduct)null;
            try
            {
                favouriteProduct = UnitOfWork.FavouriteProducts.FirstOrDefault(x => x.UserId == SystemUser.Id && x.ProductId == productId, true);
                if (favouriteProduct == null)
                {
                    favouriteProduct = new FavouriteProduct
                    {
                        ProductId = productId,
                        UserId = SystemUser.Id
                    };
                    success = UnitOfWork.FavouriteProducts.Merge(favouriteProduct);
                }
                else
                {
                    success = UnitOfWork.FavouriteProducts.DeleteOrRestore(favouriteProduct);
                }
            }
            catch (System.Exception)
            {
                success = false;
            }
            return Json(new { success, isDeleted = favouriteProduct?.IsDeleted });
        }

        [HttpPost]
        public IActionResult RemoveFromWishlist(int productId)
        {
            var success = false;
            try
            {
                if (UnitOfWork.FavouriteProducts.FirstOrDefault(x => x.UserId == SystemUser.Id && x.ProductId == productId) is FavouriteProduct favouriteProduct)
                {
                    success = UnitOfWork.FavouriteProducts.Delete(favouriteProduct);
                }
            }
            catch (System.Exception)
            {
                success = false;
            }
            return Json(new { success });
        }
    }
}