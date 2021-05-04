namespace SomeShop.Web.Extensions
{
    public static class StringExtensions
    {
        public static string Cut(this string @str, int count)
        {
            if (string.IsNullOrWhiteSpace(@str))
            {
                return string.Empty;
            }
            else if (@str.Length <= count)
            {
                return @str;
            }
            else
            {
                return @str.Substring(0, count);
            }
        }
    }
}
