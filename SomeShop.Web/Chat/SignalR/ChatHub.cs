using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SomeShop.Web.Chat.SignalR
{
    public class ChatHub : Hub
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IChatSession _chatSession;

        public ChatHub(ITelegramBotClient telegramBotClient, IChatSession chatSession)
        {
            _telegramBotClient = telegramBotClient;
            _chatSession = chatSession;
        }

        public Task Send(string message, string identifier)
        {
            return Spread(message, identifier);
        }

        public Task SendTo(string message, string identifier, string to)
        {
            return Spread(message, identifier, to);
        }

        private Task Spread(string message, string identifier, string addressee = null)
        {
            var chats = _chatSession.ChatAdministrators
                .Where(x => !long.TryParse(addressee, out var chatId) || x.ChatId == chatId);
            var sendMessageTasks = chats.Select(chat => SendRequest(chat.ChatId, message, identifier));

            return Task.WhenAll(sendMessageTasks);
        }

        private Task SendRequest(long chatId, string message, string identifier)
        {
            return _telegramBotClient.SendTextMessageAsync(
                chatId,
                ConstructRequest(message, identifier),
                ParseMode.Markdown
            );
        }

        private static string ConstructRequest(string message, string identifier)
        {
            return $"Identifier \\[{identifier}]{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   $"**Message:**{Environment.NewLine}" +
                   $"{message}";
        }
    }
}