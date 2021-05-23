using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SomeShop.Web.Chat.SignalR;

namespace SomeShop.Web.Chat
{
    public static class IdentifierStringBuilder
    {
        public const string LeftPart = "Identifier: \\[";
        public const string RightPart = "]";
        public const string Separator = ", ";
        public static readonly Regex IdentifierRegex = new Regex($"(?<={LeftPart}).*?(?={RightPart})");
        
        public static string Construct(string identifier, IdentifierType type)
        {
            var builder = new StringBuilder(LeftPart).Append(type).Append(Separator).Append(identifier).Append(RightPart);
            return builder.ToString();
        }

        public static bool TryDeconstruct(string text, out string identifier, out IdentifierType identifierType)
        {
            identifier = string.Empty;
            identifierType = default;
            
            var match = IdentifierRegex.Match(text);
            if (!match.Success)
            {
                return false;
            }
            
            var parts = match.Value.Split(Separator);
            if (!Enum.TryParse(parts.First(), false, out identifierType))
            {
                return false;
            }
            
            identifier = parts.Last();
            return  true;
        }
    }
}