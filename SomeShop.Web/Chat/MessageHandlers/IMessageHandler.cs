using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SomeShop.Web.Chat.MessageHandlers
{
    public interface IMessageHandler
    {
        Task<bool> CanHandle(Update update);
        Task<bool> HandleAsync(Update update);
    }
}