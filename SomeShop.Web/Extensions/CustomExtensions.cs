using SomeShop.DAL.Models.StoredProcedureModels;
using System.Collections.Generic;
using System.Linq;

namespace SomeShop.Web.Extensions
{
    public static class CustomExtensions
    {
        public static string GetCategoryTree(IEnumerable<GetCategoryHierarchy_Result> categories, int level = 0)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var category in categories.Where(x => x.Level == level))
            {
                if (category.IsParent)
                {
                    sb.Append("<li class=\"menu-item-has-children\"><a class=\"font-weight-bold\" href=\"");
                    sb.Append("/Shop/Index?categoryId=");
                    sb.Append(category.Id);
                    sb.Append("\">");
                    sb.Append(category.Name);
                    sb.Append("</a>");
                    sb.Append("<ul class=\"submenu2\">");
                    sb.Append(GetCategoryTree(categories.Where(x => x.Level == level + 1 && x.ParentId == category.Id || x.Level > level + 1), level + 1));
                    sb.Append("</ul>");
                }
                else
                {
                    sb.Append("<li><a href=\"");
                    sb.Append("/Shop/Index?categoryId=");
                    sb.Append(category.Id);
                    sb.Append("\">");
                    sb.Append(category.Name);
                    sb.Append("</a></li>");
                }
            }
            return sb.ToString();
        }
    }
}
