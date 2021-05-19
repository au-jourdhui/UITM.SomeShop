using Microsoft.Extensions.DependencyInjection;

namespace SomeShop.Web.Chat
{
    public static class AddChatSessionExtensions
    {
        public static IServiceCollection AddChatSession(this IServiceCollection services)
        {
            return services.AddScoped<IChatSession, ChatSession>();
        }
    }
}