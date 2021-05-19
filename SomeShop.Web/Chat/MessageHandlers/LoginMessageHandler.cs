using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SomeShop.DAL;
using SomeShop.DAL.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = SomeShop.DAL.Models.User;

namespace SomeShop.Web.Chat.MessageHandlers
{
    public class LoginMessageHandler : IMessageHandler
    {
        private const char DataSeparator = ':';

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly UnitOfWork _unitOfWork;
        private readonly string _password;

        private User _user;

        public LoginMessageHandler(
            ITelegramBotClient telegramBotClient,
            Func<UnitOfWork> unitOfWork,
            IOptions<BotSettings> botSettings)
        {
            _password = botSettings.Value?.Password ??
                        throw new ArgumentNullException(nameof(botSettings.Value.Password));

            _telegramBotClient = telegramBotClient;
            _unitOfWork = unitOfWork();
        }

        public Task<bool> CanHandle(Update update)
        {
            var message = update.Message.Text;
            if (update.Type != UpdateType.Message || string.IsNullOrWhiteSpace(message) ||
                !message.StartsWith(_password, StringComparison.Ordinal))
            {
                return Task.FromResult(false);
            }

            var parts = message.Split(DataSeparator);
            if (parts.Length != 2 ||
                !(_unitOfWork.Users.FirstOrDefault(
                        x => parts.Last().Equals(x.Email, StringComparison.OrdinalIgnoreCase))
                    is User user))
            {
                return Task.FromResult(false);
            }

            _user = user;
            return Task.FromResult(true);
        }

        public Task<bool> HandleAsync(Update update)
        {
            var chat = update.Message.Chat;
            var administrator = _unitOfWork.ChatAdministrators.FirstOrDefault(x => x.ChatId == chat.Id)
                                ?? new ChatAdministrator
                                {
                                    ChatId = chat.Id,
                                    UserId = _user.Id,
                                    UserName = chat.Username
                                };
            administrator.DateModified = DateTime.Now;

            if (!_unitOfWork.ChatAdministrators.Merge(administrator))
            {
                return Task.FromResult(false);
            }

            return SendSuccess(chat, _user);
        }

        private async Task<bool> SendSuccess(Telegram.Bot.Types.Chat chat, User user)
        {
            await _telegramBotClient.SendTextMessageAsync(chat.Id,
                $"Successfully logged in using: ```{user.Email}```",
                ParseMode.MarkdownV2);
            return true;
        }
    }
}