using System.Collections.Generic;
using System.Threading.Tasks;
using SomeShop.DAL.Models;

namespace SomeShop.Web.Chat.SignalR.Messages
{
    public interface IChatHubHistory : IEnumerable<ChatHubMessage>
    {
        ChatHubUser ChatHubUser { get; }
        ChatAdministrator ChatAdministrator { get; set; }
        ChatHubMessage First { get; }
        ChatHubMessage Last { get; }
        bool IsFinished { get; }

        void Finish();
        Task<IChatHubHistory> Push<T>(string message);
    }
}