using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public class ChatHubUserMessage : ChatHubMessage<ChatHubUser>
    {
        private readonly IChatSession _chatSession;

        public ChatHubUserMessage(
            string text,
            DateTime sentAt,
            IChatHubHistory history,
            Func<IServiceProvider> serviceProviderFactory)
            : base(text, sentAt, history)
        {
            var provider = serviceProviderFactory();

            _chatSession = provider.GetRequiredService<IChatSession>();
        }

        public override async Task SendAsync(string message)
        {
            if (IsSent || History.IsFinished)
            {
                return;
            }

            await _chatSession.SpreadAsync(message, ConnectionId);
            Sent();
        }

        public override ChatHubUser SentBy => History.ChatHubUser;

        public string ConnectionId => SentBy?.ConnectionId;

        public override bool IsUser => true;
    }
}