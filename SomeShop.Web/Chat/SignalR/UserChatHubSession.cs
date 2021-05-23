using System.Collections.Generic;

namespace SomeShop.Web.Chat.SignalR
{
    public class UserChatHubSession : IUserChatHubSession
    {
        private readonly List<ChatHubUser> _users = new();

        public IReadOnlyCollection<ChatHubUser> Users => _users.AsReadOnly();

        public bool Exists(string connectionId) => _users.Exists(x => x.ConnectionId == connectionId);

        public void Add(IdentifierType type, string identifier, string name, string connectionId)
        {
            if (_users.Exists(x => x.ConnectionId == connectionId))
            {
                return;
            }

            _users.Add(new ChatHubUser(type, identifier, name, connectionId));
        }

        public bool Remove(string connectionId)
        {
            var user = _users.Find(x => x.ConnectionId == connectionId);
            return _users.Remove(user);
        }
    }
}