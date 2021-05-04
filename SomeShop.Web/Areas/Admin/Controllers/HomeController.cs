using SomeShop.Web.Areas.Admin.Models.HomeModels;
using SomeShop.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SomeShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : AdminController
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginVM model)
        {
            bool success;
            string message = string.Empty;
            try
            {
                var password = UnitOfWork.StoredProcedures.MD5HashPassword(model.Password);
                var user = UnitOfWork.Users.FirstOrDefault(x => x.Password == password && x.Email == model.Login 
                && x.RoleId == UnitOfWork.StoredProcedures.GetUserRoleId(UserRole.Administrator.GetDescription()));
                if (success = ModelState.IsValid && user is DAL.Models.User)
                {
                    SetAuth(AuthKey, user.Token.ToString());
                }
                else if (!ModelState.IsValid)
                {
                    message = ModelState.GetHtmlErrors();
                }
                else
                {
                    message = "Wrong login or password!";
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = "An unexpected error occurred!";
            }
            return Json(new { success, message });
        }

        [HttpGet]
        [AllowAnonymous]
        public new IActionResult Logout()
        {
            base.Logout();
            return RedirectToAction("Login", "Home", new { area = "Admin" });
        }
    }
}