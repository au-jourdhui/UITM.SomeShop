using System;
using System.Threading.Tasks;

namespace SomeShop.Web.Chat.SignalR
{
    public class ChatHub : BaseHub
    {
        private readonly IUserChatHubSession _userChatHubSession;

        public ChatHub(IUserChatHubSession userChatHubSession)
        {
            _userChatHubSession = userChatHubSession;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _userChatHubSession.Remove(this.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public Task Register(string identifier, string type, string name)
        {
            if (_userChatHubSession.Exists(this.ConnectionId))
            {
                return SendBack(Methods.Info, "Already connected!");
            }

            if (!Enum.TryParse<IdentifierType>(type, true, out var identifierType))
            {
                return SendBack(Methods.Info, "Wrong identifier type!");
            }

            _userChatHubSession.Add(identifierType, identifier, name, this.ConnectionId);
            return SendBack(Methods.Info, "Successfully connected!");
        }

        public Task Send(string message)
        {
            return _userChatHubSession.FollowOrStart(message, ConnectionId);
        }

        public static class Methods
        {
            public const string Receive = nameof(Receive);
            public const string Info = nameof(Info);
        }
    }
}