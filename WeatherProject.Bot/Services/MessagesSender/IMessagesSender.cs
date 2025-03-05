using System;

namespace WeatherProject.Bot.Services.MessagesSender;

public interface IMessagesSender
{
    public Task<string> SentMessagesToAll(string? city = null);
    public Task<string> SentMessageToUser(long userId, string? city = null);
}
