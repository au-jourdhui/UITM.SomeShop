using System.Threading.Tasks;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public interface IChatHubMessage<out T>
    {
        IChatHubHistory History { get; }
        Task SendAsync(string message);
        T SentBy { get; }
    }
}