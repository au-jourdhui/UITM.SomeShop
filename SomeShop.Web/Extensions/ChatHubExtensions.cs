using Microsoft.Extensions.DependencyInjection;
using SomeShop.Web.Chat.SignalR;

namespace SomeShop.Web.Extensions
{
    public static class ChatHubExtensions
    {
        public static IServiceCollection AddUserChatHubSession(this IServiceCollection services)
        {
            return services.AddSingleton<IUserChatHubSession, UserChatHubSession>();
        }
    }
}