using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat.SignalR
{
    public class ChatHub : BaseHub
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IChatSession _chatSession;
        private readonly IUserChatHubSession _userChatHubSession;

        public ChatHub(
            ITelegramBotClient telegramBotClient,
            IChatSession chatSession,
            IUserChatHubSession userChatHubSession)
        {
            _telegramBotClient = telegramBotClient;
            _chatSession = chatSession;
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
            return Spread(message);
        }

        public Task SendTo(string message, string to)
        {
            return Spread(message, to);
        }

        #region Helpers

        private Task Spread(string message, string addressee = null)
        {
            if (string.IsNullOrWhiteSpace(message) || !_userChatHubSession.Exists(this.ConnectionId))
            {
                return Task.CompletedTask;
            }
            
            var chatHubUser = _userChatHubSession.Users.First(x => x.ConnectionId == this.ConnectionId);
            
            var chats = _chatSession.ChatAdministrators
                .Where(x => !long.TryParse(addressee, out var chatId) || x.ChatId == chatId);
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

        #endregion

        public static class Methods
        {
            public const string Receive = nameof(Receive);
            public const string Info = nameof(Info);
        }
    }
}