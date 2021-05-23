using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SomeShop.DAL;
using SomeShop.Web.Chat.SignalR;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat.MessageHandlers
{
    public class RequestReplyMessageHandler : IMessageHandler
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserChatHubSession _userChatHubSession;
        private readonly IChatSession _chatSession;
        private readonly Func<UnitOfWork> _uowFactory;

        private ChatHubUser _chatHubUser;

        public RequestReplyMessageHandler(
            IHubContext<ChatHub> hubContext, 
            IUserChatHubSession userChatHubSession,
            IChatSession chatSession,
            Func<UnitOfWork> uowFactory)
        {
            _hubContext = hubContext;
            _userChatHubSession = userChatHubSession;
            _chatSession = chatSession;
            _uowFactory = uowFactory;
        }

        public Task<bool> CanHandle(Update update)
        {
            if (!ValidateReplyMessageType(update.Message.Type))
            {
                return Task.FromResult(false);
            }

            if (!(update.Message.ReplyToMessage is { } reply))
            {
                return Task.FromResult(false);
            }

            if (!IdentifierStringBuilder.TryDeconstruct(reply.Text, out var user))
            {
                return Task.FromResult(false);
            }

            _chatHubUser = _userChatHubSession.Users.FirstOrDefault(x => x.Identifier == user.Identifier
                                                                         && x.IdentifierType == user.IdentifierType);
            return Task.FromResult(_chatHubUser is not null);
        }

        public async Task<bool> HandleAsync(Update update)
        {
            var client = _hubContext.Clients.Client(_chatHubUser?.ConnectionId);
            if (client is null)
            {
                return false;
            }

            var administrator = _chatSession.ChatAdministrators.FirstOrDefault(x => x.ChatId == update.Message.Chat.Id);
            var user = _uowFactory().Users.FindById(administrator?.UserId);
            
            await client.SendAsync(ChatHub.Methods.Receive, update.Message.Text, user?.FirstName ?? "Operator");
            return true;
        }

        private static bool ValidateReplyMessageType(MessageType type)
        {
            return type switch
            {
                MessageType.Text => true,
                _ => false
            };
        }
    }
}