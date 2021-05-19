using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = SomeShop.DAL.Models.User;

namespace SomeShop.Web.Chat.MessageHandlers
{
    public class LoginMessageHandler : IMessageHandler, IAllowAnonymous
    {
        private const char DataSeparator = ':';

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IChatSession _chatSession;

        private User _user;

        public LoginMessageHandler(
            ITelegramBotClient telegramBotClient,
            IChatSession chatSession)
        {
            _telegramBotClient = telegramBotClient;
            _chatSession = chatSession;
        }

        public Task<bool> CanHandle(Update update)
        {
            if (_chatSession.ChatAdministrators.Any(x => x.ChatId == update.Message.Chat.Id))
            {
                return Task.FromResult(false);
            }
            
            var message = update.Message.Text;
            if (string.IsNullOrWhiteSpace(message))
            {
                return Task.FromResult(false);
            }

            var parts = message.Split(DataSeparator);
            if (parts.Length != 2)
            {
                return Task.FromResult(false);
            }

            var login = parts.Last();
            var password = parts.First();
            if (!(_chatSession.Login(update.Message.Chat, login, password) is User user))
            {
                return Task.FromResult(false);
            }

            _user = user;
            return Task.FromResult(true);
        }

        public Task<bool> HandleAsync(Update update)
        {
            var chat = update.Message.Chat;
            var message = update.Message;
            _chatSession.Register(chat, _user);
            return SendSuccess(message, _user);
        }

        private async Task<bool> SendSuccess(Message message, User user)
        {
            var deleteTask = _telegramBotClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            var sendTask = _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                $"Successfully logged in using: ```{user.Email}```",
                ParseMode.MarkdownV2);

            await Task.WhenAll(deleteTask, sendTask);
            return true;
        }
    }
}