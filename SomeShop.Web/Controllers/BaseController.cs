using SomeShop.DAL;
using SomeShop.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SomeShop.Web.Controllers
{
    public class BaseController : Controller
    {
        public const string AuthKey = "__auth__Cosmetics__";

        protected User SystemUser { get; set; }
        protected bool Anonymous => !(SystemUser is User user && user.Id != 0 && user.Token != Guid.Empty);
        protected UnitOfWork UnitOfWork { get; private set; }

        public BaseController()
        {
            UnitOfWork = new UnitOfWork(new SqlConnection(GlobalVariables.ConnectionString));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ShouldRedirectToRegister(string url, string method)
        {
            var keys = url.Substring(0, url.IndexOf('?') == -1 ? url.Length : url.IndexOf('?')).Trim('/').Split('/');
            var asm = Assembly.GetExecutingAssembly();
            var methods = asm.GetTypes()
                             .Where(type => typeof(Controller).IsAssignableFrom(type) && keys.Contains(type.Name.Replace("Controller", "")))
                             .SelectMany(type => type.GetMethods())
                             .Where(m => m.IsPublic && keys.Contains(m.Name) && (m.IsDefined(typeof(AllowAnonymousAttribute)) || 
                             m.CustomAttributes.Any(x => ((x as object as HttpMethodAttribute)?.HttpMethods
                             .Any(h => h.ToUpper() == method.ToUpper())).GetValueOrDefault())));
            return Json(new { should = !(methods?.Count() > 0) && Anonymous });
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authValue = GetAuth(AuthKey);
            if (Guid.TryParse(authValue, out Guid token))
            {
                SystemUser = UnitOfWork.Users.FindByToken(token);
            }

            ViewBag.CategoryHierarchy = UnitOfWork.StoredProcedures.GetCategoryHierarchy();
            ViewBag.SystemUser = SystemUser;
            ViewBag.Anonymous = Anonymous;
            ViewBag.ContactInfo = UnitOfWork.ContactInfo.FindAll();

            var allowAnonymous = ((context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.CustomAttributes.Any(x => x.AttributeType == typeof(AllowAnonymousAttribute))).GetValueOrDefault();
            if (allowAnonymous || !ViewBag.Anonymous)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = RedirectToAction("Login", "Account");
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
