using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SomeShop.DAL;
using SomeShop.DAL.Models;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public class ChatHubAdministratorMessage : ChatHubMessage<ChatAdministrator>
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserChatHubSession _userChatHubSession;
        private readonly Func<UnitOfWork> _uowFactory;
        private string _name;

        public ChatHubAdministratorMessage(
            string text,
            DateTime sentAt,
            IChatHubHistory history,
            Func<IServiceProvider> serviceProviderFactory)
            : base(text, sentAt, history)
        {
            var provider = serviceProviderFactory();


            _hubContext = provider.GetRequiredService<IHubContext<ChatHub>>();
            _userChatHubSession = provider.GetRequiredService<IUserChatHubSession>();
            _uowFactory = provider.GetRequiredService<Func<UnitOfWork>>();
        }

        public override async Task SendAsync(string message)
        {
            var chatHubUser = _userChatHubSession.Users
                .FirstOrDefault(x => x.Identifier == History.ChatHubUser.Identifier
                                     && x.IdentifierType == History.ChatHubUser.IdentifierType);

            var client = _hubContext.Clients.Client(chatHubUser?.ConnectionId);
            if (client is null)
            {
                return;
            }

            await client.SendAsync(ChatHub.Methods.Receive, message, Name);
            Sent();
        }

        public override ChatAdministrator SentBy => History.ChatAdministrator;

        public override bool IsUser => false;

        public override string Name => _name ??= _uowFactory().Users.FindById(SentBy?.UserId)?.FirstName ?? "Operator";
    }
}