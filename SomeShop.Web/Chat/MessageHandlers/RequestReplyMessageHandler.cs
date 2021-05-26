using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeShop.Web.Chat.SignalR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat.MessageHandlers
{
    public class RequestReplyMessageHandler : IMessageHandler
    {
        private readonly IUserChatHubSession _userChatHubSession;
        private readonly IChatSession _chatSession;
        private readonly ITelegramBotClient _telegramBotClient;

        private ChatHubUser _chatHubUser;

        public RequestReplyMessageHandler(
            IUserChatHubSession userChatHubSession,
            IChatSession chatSession,
            ITelegramBotClient telegramBotClient)
        {
            _userChatHubSession = userChatHubSession;
            _chatSession = chatSession;
            _telegramBotClient = telegramBotClient;
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
            var chatId = update.Message.Chat.Id;
            var administrator = _chatSession.ChatAdministrators.FirstOrDefault(x => x.ChatId == chatId);

            var tasks = new List<Task>(
                new[]
                {
                    _userChatHubSession.ReplyToUser(update.Message.Text, _chatHubUser.ConnectionId, administrator)
                }
            );
            
            if (!_userChatHubSession.HasOpenConversation(chatId))
            {
                tasks.Add(
                    _telegramBotClient.SendTextMessageAsync(
                        chatId,
                        $"Chat with *{_chatHubUser.Name}* has been started!"
                    )
                );
            }

            await Task.WhenAll(tasks);
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