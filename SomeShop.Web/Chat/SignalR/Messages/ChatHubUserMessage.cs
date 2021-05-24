using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public class ChatHubUserMessage : ChatHubMessage<ChatHubUser>
    {
        private readonly IUserChatHubSession _userChatHubSession;
        private readonly IChatSession _chatSession;
        private readonly ITelegramBotClient _telegramBotClient;

        public ChatHubUserMessage(
            string text,
            DateTime sentAt,
            IChatHubHistory history,
            Func<IServiceProvider> serviceProviderFactory)
            : base(text, sentAt, history)
        {
            var provider = serviceProviderFactory();

            _userChatHubSession = provider.GetRequiredService<IUserChatHubSession>();
            _chatSession = provider.GetRequiredService<IChatSession>();
            _telegramBotClient = provider.GetRequiredService<ITelegramBotClient>();
        }

        public override async Task SendAsync(string message)
        {
            if (IsSent || History.IsFinished)
            {
                return;
            }

            await Spread(message, History.ChatAdministrator?.ChatId);
            Sent();
        }

        public override ChatHubUser SentBy => History.ChatHubUser;

        public string ConnectionId => SentBy?.ConnectionId;

        private Task Spread(string message, long? chatId = null)
        {
            if (string.IsNullOrWhiteSpace(message) || !_userChatHubSession.Exists(ConnectionId))
            {
                return Task.CompletedTask;
            }

            if (!(_userChatHubSession.Users.FirstOrDefault(x => x.ConnectionId == ConnectionId) is { } chatHubUser))
            {
                return Task.CompletedTask;
            }

            var chats = _chatSession.ChatAdministrators
                .Where(x => !chatId.HasValue || x.ChatId == chatId.Value);
            var sendMessageTasks = chats.Select(chat => SendRequest(chat.ChatId, message, chatHubUser));

            return Task.WhenAll(sendMessageTasks);
        }

        private Task SendRequest(long chatId, string message, ChatHubUser chatHubUser)
        {
            return _telegramBotClient.SendTextMessageAsync(
                chatId,
                ConstructRequest(message, chatHubUser),
                ParseMode.Markdown
            );
        }

        private static string ConstructRequest(string message, ChatHubUser user)
        {
            return $"{IdentifierStringBuilder.Construct(user)}" +
                   $"{Environment.NewLine}{Environment.NewLine}" +
                   $"**Message:**{Environment.NewLine}" +
                   $"{message}";
        }

        public override bool IsUser => true;
    }
}