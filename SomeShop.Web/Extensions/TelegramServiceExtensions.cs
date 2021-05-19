using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomeShop.Web.Chat;
using Telegram.Bot;

namespace SomeShop.Web.Extensions
{
    public static class TelegramServiceExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var botSettingsSection = configuration.GetSection(nameof(BotSettings));
            var botSettings = botSettingsSection.Get<BotSettings>();

            if (string.IsNullOrWhiteSpace(botSettings.Token))
            {
                throw new ArgumentNullException(nameof(botSettings.Token));
            }

            if (string.IsNullOrWhiteSpace(botSettings.Password))
            {
                throw new ArgumentNullException(nameof(botSettings.Password));
            }

            serviceCollection.Configure<BotSettings>(botSettingsSection);
            return serviceCollection.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botSettings.Token));
        }
    }
}