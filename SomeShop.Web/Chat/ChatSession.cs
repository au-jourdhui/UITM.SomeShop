using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using SomeShop.DAL;
using SomeShop.DAL.Models;

namespace SomeShop.Web.Chat
{
    public class ChatSession : IChatSession
    {
        private readonly string _password;
        private readonly Func<UnitOfWork> _uowFactory;
        private readonly List<ChatAdministrator> _administrators;

        public ChatSession(
            Func<UnitOfWork> uowFactory,
            IOptions<BotSettings> botSettings)
        {
            _password = botSettings.Value?.Password ??
                        throw new ArgumentNullException(nameof(botSettings.Value.Password));

            _administrators = new List<ChatAdministrator>();
            _uowFactory = uowFactory;
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
    }
}