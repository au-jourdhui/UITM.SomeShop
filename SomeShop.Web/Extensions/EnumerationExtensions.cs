using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SomeShop.Web.Extensions
{
    public static class EnumerationExtensions
    {
        public static string GetDescription<T>(this T value)
        {
                var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
                var descriptionAttribute =
                    enumMember == null
                        ? default(DescriptionAttribute)
                        : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                return
                    descriptionAttribute == null
                        ? value.ToString()
                        : descriptionAttribute.Description;
        }
    }
}
