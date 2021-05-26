using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SomeShop.DAL;
using SomeShop.DAL.Models;
using SomeShop.Web.Chat.SignalR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat
{
    public class ChatSession : IChatSession
    {
        private readonly string _password;
        private readonly Func<UnitOfWork> _uowFactory;
        private readonly IUserChatHubSession _userChatHubSession;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly List<ChatAdministrator> _administrators;

        public ChatSession(
            Func<UnitOfWork> uowFactory,
            IOptions<BotSettings> botSettings,
            IUserChatHubSession userChatHubSession,
            ITelegramBotClient telegramBotClient)
        {
            _password = botSettings.Value?.Password ??
                        throw new ArgumentNullException(nameof(botSettings.Value.Password));

            _administrators = new List<ChatAdministrator>();
            _uowFactory = uowFactory;
            _userChatHubSession = userChatHubSession;
            _telegramBotClient = telegramBotClient;
        }

        public IReadOnlyCollection<ChatAdministrator> ChatAdministrators => _administrators.AsReadOnly();

        public User Login(Telegram.Bot.Types.Chat chat, string email, string password)
        {
            var uow = _uowFactory();
            if (_administrators.Find(x => x.ChatId == chat.Id) is { } signed)
            {
                return uow.Users.FindById(signed.UserId);
            }

            var user = _password.Equals(password, StringComparison.Ordinal)
                ? uow.Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                : null;

            if (user != null &&
                uow.ChatAdministrators.FirstOrDefault(x => x.ChatId == chat.Id) is { } administrator)
            {
                _administrators.Add(administrator);
            }

            return user;
        }

        public void Register(Telegram.Bot.Types.Chat chat, User user)
        {
            if (_administrators.Any(x => x.ChatId == chat.Id))
            {
                return;
            }

            var uow = _uowFactory();
            var administrator = uow.ChatAdministrators.FirstOrDefault(x => x.ChatId == chat.Id)
                                ?? new ChatAdministrator
                                {
                                    ChatId = chat.Id,
                                    UserId = user.Id,
                                    UserName = chat.Username
                                };
            administrator.DateModified = DateTime.Now;
            _administrators.Add(administrator);
            uow.ChatAdministrators.Merge(administrator);
        }

        public Task SpreadAsync(string message, string connectionId)
        {
            return Spread(message, connectionId);
        }
        
        private Task Spread(string message, string connectionId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return Task.CompletedTask;
            }

            if (!(_userChatHubSession.Users.FirstOrDefault(x => x.ConnectionId == connectionId) is { } chatHubUser))
            {
                return Task.CompletedTask;
            }

            var chatId = _userChatHubSession.GetCurrentHistory(connectionId)?.ChatAdministrator?.ChatId;
            var chats = ChatAdministrators
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
    }
}