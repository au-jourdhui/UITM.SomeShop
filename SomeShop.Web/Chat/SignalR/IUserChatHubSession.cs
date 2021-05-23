using System.Collections.Generic;

namespace SomeShop.Web.Chat.SignalR
{
    public interface IUserChatHubSession
    {
        IReadOnlyCollection<ChatHubUser> Users { get; }

        bool Exists(string connectionId);

        void Add(IdentifierType type, string identifier, string connectionId);
        
        bool Remove(string connectionId);
    }
}