using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SomeShop.Web.Chat.MessageHandlers;

namespace SomeShop.Web.Chat
{
    public static class AddMessageHandlersExtension
    {
        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            var typeInfos = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(x => x.ImplementedInterfaces.Contains(typeof(IMessageHandler))).ToList();

            foreach (var typeInfo in typeInfos)
            {
                if (!typeInfo.Name.EndsWith(nameof(IMessageHandler).Substring(1)))
                {
                    throw new ArgumentException("Incorrect message handler naming!");
                }
                services.AddScoped(typeof(IMessageHandler), typeInfo);
            }

            return services;
        }
    }
}