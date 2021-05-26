using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SomeShop.Web.Chat.SignalR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat.MessageHandlers
{
    public class FinishConversationMessageHandler : IMessageHandler
    {
        private static readonly Regex RegexCommand = new("^\\/finish$");
        private static readonly Random Random = new();

        private static readonly string[] LastMessageTemplates =
        {
            "Sending you good vibes!",
            "Looking forward to hearing from you!",
            "Eager to continue our collaboration!",
            "Our pleasure to help you!"
        };

        private static string LastMessageTemplate => LastMessageTemplates[Random.Next(LastMessageTemplates.Length)];

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IUserChatHubSession _userChatHubSession;

        public FinishConversationMessageHandler(
            ITelegramBotClient telegramBotClient,
            IUserChatHubSession userChatHubSession)
        {
            _telegramBotClient = telegramBotClient;
            _userChatHubSession = userChatHubSession;
        }

        public Task<bool> CanHandle(Update update)
        {
            return Task.FromResult(
                ValidateReplyMessageType(update.Message.Type)
                && RegexCommand.IsMatch(update.Message.Text)
                && _userChatHubSession.HasOpenConversation(update.Message.Chat.Id)
            );
        }

        public async Task<bool> HandleAsync(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var history = _userChatHubSession.GetCurrentHistory(chatId);

            if (!await _userChatHubSession.FinishConversation(chatId, LastMessageTemplate))
            {
                return false;
            }

            await _telegramBotClient.SendTextMessageAsync(
                chatId,
                $"Your conversation with *{history.ChatHubUser.Name}* is finished!",
                ParseMode.Markdown
            );

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