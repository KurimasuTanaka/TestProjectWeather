using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using WeatherProject.Bot.Services.MessagesSender;
using WeatherProject.WeatherAPI;

namespace WeatherProject.Bot;

public static class TelegramBotCollectionExtension
{
    // This method extends IServiceCollection and registers services related to the Item API
    public static IServiceCollection AddWeatherTelegramBot(this IServiceCollection services, string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException(nameof(token), "Token is required for the bot to work");
        }

        services.AddHttpClient("tgwebhook").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>(
            httpClient => new TelegramBotClient(token, httpClient));
            
        services.AddScoped<IMessagesSender, MessagesSender>();
        services.AddScoped<IUpdateHandler, UpdateHandler>();
        services.AddScoped<IWeatherDataReceiver, WeatherDataReceiver>();

        services.ConfigureTelegramBotMvc();
        return services; // Return the IServiceCollection for method chaining
    }

}
