using System;
using System.Threading.Tasks;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public abstract class ChatHubMessage
    {
        protected ChatHubMessage(string text, DateTime sentAt)
        {
            Text = text;
            SentAt = sentAt;
        }
        
        public string Text { get; }
        
        public DateTime SentAt { get; }
        

        public abstract bool IsUser { get; }
        public virtual string Name => string.Empty;
        
        public bool IsSent { get; private set; }

        public void Sent() => IsSent = false;
    }
    
    public abstract class ChatHubMessage<T> : ChatHubMessage, IChatHubMessage<T>
    {
        protected ChatHubMessage(string text, DateTime sentAt, IChatHubHistory history) : base(text, sentAt)
        {
            History = history;
        }
        
        public IChatHubHistory History { get; }
        
        public abstract T SentBy { get; }
        
        public abstract Task SendAsync(string message);
    }
}