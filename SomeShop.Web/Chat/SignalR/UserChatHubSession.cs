using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SomeShop.DAL.Models;
using SomeShop.Web.Chat.SignalR.Messages;

namespace SomeShop.Web.Chat.SignalR
{
    public class UserChatHubSession : IUserChatHubSession
    {
        private readonly Func<IServiceProvider> _serviceProviderFactory;
        private readonly List<ChatHubUser> _users = new();
        private readonly List<IChatHubHistory> _histories = new();

        public UserChatHubSession(IServiceProvider serviceProvider)
        {
            _serviceProviderFactory = serviceProvider.GetRequiredService<IServiceProvider>;
        }

        public IReadOnlyCollection<ChatHubUser> Users => _users.AsReadOnly();
        public IReadOnlyCollection<IChatHubHistory> Histories => _histories.AsReadOnly();

        public bool Exists(string connectionId) => _users.Exists(x => x.ConnectionId == connectionId);

        public void Add(IdentifierType type, string identifier, string name, string connectionId)
        {
            if (_users.Exists(x => x.ConnectionId == connectionId))
            {
                return;
            }

            if (_users.Find(x => x.IdentifierType == type && x.Identifier == identifier) is { } user)
            {
                user.ConnectionId = connectionId;
                return;
            }

            _users.Add(new ChatHubUser(type, identifier, name, connectionId));
        }

        public bool Remove(string connectionId)
        {
            var user = _users.Find(x => x.ConnectionId == connectionId);
            if (user is null)
            {
                return true;
            }

            user.ConnectionId = null;
            return false;
        }

        public async Task<IChatHubHistory> FollowOrStart(string message, string connectionId)
        {
            if (!(_users.Find(x => x.ConnectionId == connectionId) is { } user))
            {
                return default;
            }

            if (_histories.FirstOrDefault(x => !x.IsFinished && x.ChatHubUser.ConnectionId == connectionId) is
                { } history)
            {
                return await history.Push<ChatHubUser>(message);
            }

            history = await ChatHubHistory.Create(message, user, _serviceProviderFactory);
            _histories.Add(history);
            return history;
        }

        public Task<IChatHubHistory> ReplyToUser(string message,
            string connectionId,
            ChatAdministrator chatAdministrator)
        {
            if (!_users.Exists(x => x.ConnectionId == connectionId) ||
                !(_histories.FirstOrDefault(x => !x.IsFinished && x.ChatHubUser.ConnectionId == connectionId) is
                    { } history))
            {
                return Task.FromResult(default(IChatHubHistory));
            }

            history.ChatAdministrator = chatAdministrator;
            return history.Push<ChatAdministrator>(message);
        }

        public Task<IChatHubHistory> ReplyToUser(string message, IChatHubHistory history) =>
            ReplyToUser(message, history.ChatHubUser.ConnectionId, history.ChatAdministrator);

        public IChatHubHistory GetCurrentHistory(string identifier, IdentifierType identifierType)
        {
            return _histories.FirstOrDefault(x =>
                !x.IsFinished
                && x.ChatHubUser.Identifier == identifier
                && x.ChatHubUser.IdentifierType == identifierType);
        }

        public IChatHubHistory GetCurrentHistory(string connectionId) =>
            _histories.FirstOrDefault(x => !x.IsFinished && x.ChatHubUser.ConnectionId == connectionId);

        public IChatHubHistory GetCurrentHistory(long chatId) =>
            _histories.FirstOrDefault(x => !x.IsFinished && x.ChatAdministrator.ChatId == chatId);

        public bool HasOpenConversation(long chatId) =>
            _histories.Any(x => !x.IsFinished && x.ChatAdministrator?.ChatId == chatId);

        public async Task<bool> FinishConversation(long chatId, string message)
        {
            if (GetCurrentHistory(chatId) is not { } history)
                return false;

            (await ReplyToUser(message, history)).Finish();
            return true;
        }
    }
}