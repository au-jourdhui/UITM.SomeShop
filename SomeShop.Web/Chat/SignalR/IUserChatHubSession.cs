using System.Collections.Generic;
using System.Threading.Tasks;
using SomeShop.DAL.Models;
using SomeShop.Web.Chat.SignalR.Messages;

namespace SomeShop.Web.Chat.SignalR
{
    public interface IUserChatHubSession
    {
        IReadOnlyCollection<ChatHubUser> Users { get; }
        IReadOnlyCollection<IChatHubHistory> Histories { get; }

        bool Exists(string connectionId);

        void Add(IdentifierType type, string identifier, string name, string connectionId);
        
        bool Remove(string connectionId);

        Task<IChatHubHistory> FollowOrStart(string message, string connectionId);
        Task<IChatHubHistory> ReplyToUser(string message, string connectionId, ChatAdministrator chatAdministrator);
        IChatHubHistory GetCurrentHistory(string identifier, IdentifierType identifierType);
    }
}