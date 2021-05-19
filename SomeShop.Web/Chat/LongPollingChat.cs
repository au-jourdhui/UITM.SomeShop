using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SomeShop.Web.Chat.MessageHandlers;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace SomeShop.Web.Chat
{
    public class LongPollingChat : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITelegramBotClient _client;
        private readonly IChatSession _chatSession;

        public LongPollingChat(
            IServiceScopeFactory serviceScopeFactory,
            ITelegramBotClient client,
            IChatSession chatSession)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _client = client;
            _chatSession = chatSession;

            _client.OnUpdate += ClientOnOnUpdate;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
                await SendMessageToAllAdministrators("System is up! Please log in to the system...");
                _client.StartReceiving(cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await SendMessageToAllAdministrators("System is temporarily down...");
            _client.StopReceiving();
        }

        private async void ClientOnOnUpdate(object sender, UpdateEventArgs e)
        {
            var update = e.Update;
            var handlers = GetMessageHandlers().ToList();

            var canHandleTasks = handlers.Select(x => CanHandlingTask(x, e.Update)).ToList();

            var results = (await Task.WhenAll(canHandleTasks)).Where(x => x.IsSuccess).ToList();
            
            if (results.Count == 0)
            {
                await _client.SendTextMessageAsync(update.Message.Chat.Id, "Unknown command!");
            }
            else if (results.Count == 1)
            {
                await results.First().Handler.HandleAsync(update);
            }
            else
            {
                var names = results.Select(x =>
                    x.Handler.GetType().Name.Replace(nameof(IMessageHandler).Substring(1), string.Empty));
                var message = $"Ambiguous command: {string.Join(", ", names)}";
                await _client.SendTextMessageAsync(update.Message.Chat.Id, message);
            }
        }

        private async Task<(bool IsSuccess, IMessageHandler Handler)> CanHandlingTask(IMessageHandler handler, Update update)
        {
            var isAnonymous = handler is IAllowAnonymous;
            if (!isAnonymous && _chatSession.ChatAdministrators.All(c => c.ChatId != update.Message.Chat.Id))
            {
                return (false, null);
            }

            var result = await handler.CanHandle(update);
            return (result, handler);
        }

        private Task SendMessageToAllAdministrators(string message)
        {
            var chats =  _chatSession.ChatAdministrators.Select(x => x.ChatId).ToList();
            if (!chats.Any())
            {
                return Task.CompletedTask;
            }

            var tasks = chats.Select(x => _client.SendTextMessageAsync(x, message));
            return Task.WhenAll(tasks);
        }

        private IEnumerable<IMessageHandler> GetMessageHandlers()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetServices<IMessageHandler>();
            }
        }
    }
}