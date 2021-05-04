using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace SomeShop.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetHtmlErrors(this ModelStateDictionary modelState)
        {
            var sb = new StringBuilder("<ul>");
            foreach (var value in modelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    sb.Append("<li>- ");
                    sb.Append(error.ErrorMessage);
                    sb.Append("</li>");
                }
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}
