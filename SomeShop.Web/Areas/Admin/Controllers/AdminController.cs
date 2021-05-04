using SomeShop.DAL;
using SomeShop.DAL.Models;
using SomeShop.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SomeShop.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public const string AuthKey = "__auth__admin__Cosmetics__";

        protected User SystemUser { get; set; }
        protected bool Anonymous => !(SystemUser is User user && user.Id != 0 && user.Token != Guid.Empty);
        protected UnitOfWork UnitOfWork { get; private set; }

        public AdminController()
        {
            UnitOfWork = new UnitOfWork(new SqlConnection(GlobalVariables.ConnectionString));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authValue = GetAuth(AuthKey);
            if (Guid.TryParse(authValue, out Guid token))
            {
                var user = UnitOfWork.Users.FindByToken(token);
                SystemUser = user?.RoleId == UnitOfWork.StoredProcedures.GetUserRoleId(UserRole.Administrator.GetDescription()) ? user : null;
            }
            ViewBag.SystemUser = SystemUser;

            var allowAnonymous = ((context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.CustomAttributes.Any(x => x.AttributeType == typeof(AllowAnonymousAttribute))).GetValueOrDefault();
            if (allowAnonymous || !Anonymous)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = RedirectToAction("Login", "Home");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            UnitOfWork.Dispose();
        }

        protected void Login(string token)
        {
            if (!Anonymous)
            {
                SetAuth(AuthKey, token);
            }
        }
        protected void Logout()
        {
            if (!Anonymous)
            {
                RemoveAuth(AuthKey);
            }
        }

        protected string GetAuth(string key)
        {
            return HttpContext.Session.GetString(key);
        }
        protected void SetAuth(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }
        protected void RemoveAuth(string key)
        {
            HttpContext.Session.Remove(key);
        }
    }
}
