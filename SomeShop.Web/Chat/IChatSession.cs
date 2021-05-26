using System.Collections.Generic;
using System.Threading.Tasks;
using SomeShop.DAL.Models;

namespace SomeShop.Web.Chat
{
    public interface IChatSession
    {
        IReadOnlyCollection<ChatAdministrator> ChatAdministrators { get; }

        User Login(Telegram.Bot.Types.Chat chat, string email, string password);
        
        void Register(Telegram.Bot.Types.Chat chat, User user);

        Task SpreadAsync(string message, string connectionId);
    }
}