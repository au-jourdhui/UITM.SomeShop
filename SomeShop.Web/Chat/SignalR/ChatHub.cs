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

        public Task Register(string identifier, string type)
        {
            if (_userChatHubSession.Exists(this.ConnectionId))
            {
                return SendBack(Methods.Receive, "Already connected!");
            }

            if (!Enum.TryParse<IdentifierType>(type, true, out var identifierType))
            {
                return SendBack(Methods.Receive, "Wrong identifier type!");
            }

            _userChatHubSession.Add(identifierType, identifier, this.ConnectionId);
            return SendBack(Methods.Receive, "Successfully connected!");
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
            if (!_userChatHubSession.Exists(this.ConnectionId))
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
                ConstructRequest(message, chatHubUser.Identifier, chatHubUser.IdentifierType),
                ParseMode.Markdown
            );
        }

        private static string ConstructRequest(string message, string identifier, IdentifierType type)
        {
            return $"{IdentifierStringBuilder.Construct(identifier, type)}{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   $"**Message:**{Environment.NewLine}" +
                   $"{message}";
        }

        #endregion

        public static class Methods
        {
            public const string Receive = nameof(Receive);
        }
    }
}