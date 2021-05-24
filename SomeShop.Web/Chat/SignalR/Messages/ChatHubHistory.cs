using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SomeShop.DAL.Models;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public class ChatHubHistory : IChatHubHistory
    {
        private readonly Func<IServiceProvider> _serviceProviderFactory;
        private static readonly IEnumerable<Type> MessageTypes;
        private readonly LinkedList<ChatHubMessage> _messages = new();

        static ChatHubHistory()
        {
            MessageTypes = Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(x => !x.IsAbstract 
                            &&  x.DeclaredConstructors.Any(c => c.IsPublic) 
                            && typeof(ChatHubMessage).IsAssignableFrom(x.AsType()))
                .ToList();
        }

        private ChatHubHistory(string message, ChatHubUser chatHubUser, Func<IServiceProvider> serviceProviderFactory)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            _serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
            ChatHubUser = chatHubUser ?? throw new ArgumentNullException(nameof(chatHubUser));
        }

        public static Task<IChatHubHistory> Create(string message, ChatHubUser chatHubUser, Func<IServiceProvider> serviceProviderFactory)
        {
            var chatHubHistory = new ChatHubHistory(message, chatHubUser, serviceProviderFactory);
            return chatHubHistory.Push<ChatHubUser>(message);
        }
        
        public ChatHubUser ChatHubUser { get; }
        
        public ChatAdministrator ChatAdministrator { get; set; }

        public ChatHubMessage First => _messages.First!.Value;

        public ChatHubMessage Last => _messages.Last?.Value;
        
        public bool IsFinished { get; private set; }

        public void Finish()
        {
            IsFinished = true;
        }

        public async Task<IChatHubHistory> Push<T>(string message)
        {
            var chatHubMessage = CreateMessage<T>(message, DateTime.UtcNow);
            await chatHubMessage.SendAsync(message);
            _messages.AddLast(chatHubMessage);
            return this;
        }

        public IEnumerator<ChatHubMessage> GetEnumerator()
        {
            return _messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private ChatHubMessage<T> CreateMessage<T>(string text, DateTime date)
        {
            var messageType = MessageTypes.First(x => x.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IChatHubMessage<T>)));
            return Activator.CreateInstance(messageType, text, date, this, _serviceProviderFactory) as ChatHubMessage<T>;
        }
    }
}