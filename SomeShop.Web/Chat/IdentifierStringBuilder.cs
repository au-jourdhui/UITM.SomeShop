using System;
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
        public static readonly Regex IdentifierRegex = new($"(?<={LeftPart}).*?(?={RightPart})");

        public static string Construct(ChatHubUser user)
        {
            var builder = new StringBuilder(LeftPart)
                .Append(user.IdentifierType)
                .Append(Separator)
                .Append(user.Identifier)
                .Append(Separator)
                .Append(user.Name)
                .Append(RightPart);
            return builder.ToString();
        }

        public static bool TryDeconstruct(string text, out ChatHubUser user)
        {
            user = ChatHubUser.Empty;

            var match = IdentifierRegex.Match(text);
            if (!match.Success)
            {
                return false;
            }

            var parts = match.Value.Split(Separator);
            if (parts.Length != 3)
            {
                return false;
            }
            
            if (!Enum.TryParse(parts[0], false, out IdentifierType identifierType))
            {
                return false;
            }

            user = new ChatHubUser(identifierType, parts[1], parts[2]);
            return true;
        }
    }
}