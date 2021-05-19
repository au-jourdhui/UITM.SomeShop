using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SomeShop.DAL;
using SomeShop.DAL.Models;
using SomeShop.Web.Chat.MessageHandlers;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace SomeShop.Web.Chat
{
    public class LongPollingChat : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITelegramBotClient _client;
        private readonly Func<UnitOfWork> _unitOfWorkFactory;
        private ICollection<ChatAdministrator> _administrators;

        public LongPollingChat(IServiceScopeFactory serviceScopeFactory, ITelegramBotClient client, Func<UnitOfWork> unitOfWorkFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _client = client;
            _unitOfWorkFactory = unitOfWorkFactory;

            _client.OnUpdate += ClientOnOnUpdate;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var unitOfWork = _unitOfWorkFactory())
            {
                _administrators = unitOfWork.ChatAdministrators.FindAll().ToList();
                await SendMessageToAllAdministrators("System is up! Please log in to the system...");
                _client.StartReceiving(cancellationToken: cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await SendMessageToAllAdministrators("System is temporarily down...");
            _client.StopReceiving();
        }


        public async void Dispose()
        {
            await SendMessageToAllAdministrators("System is temporarily down...");
        }
        
        private async void ClientOnOnUpdate(object sender, UpdateEventArgs e)
        {
            var update = e.Update;
            var handlers = GetMessageHandlers().ToList();

            var canHandleTasks = handlers.Select(async x =>
            {
                var result = await x.CanHandle(update);
                return (IsSuccess: result, Handler: x);
            }).ToList();

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

        private Task SendMessageToAllAdministrators(string message)
        {
            var chats = _administrators.Select(x => x.ChatId).Distinct().ToList();
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