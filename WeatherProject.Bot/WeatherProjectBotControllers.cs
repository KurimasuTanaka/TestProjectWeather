using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace WeatherProject.Bot;

[ApiController]
public class WeatherProjectBotControllers(IConfiguration _configuration) : ControllerBase
{

    [HttpGet("setWebhook")] //Setting up webhook for the bot
    public async Task<string> SetWebHook([FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        // var webhookUrl = Config.Value.BotWebhookUrl.AbsoluteUri;
        var webhookUrl = $"{_configuration["TelegramBot:WebHook"]}/bot";    
        await bot.SetWebhook(webhookUrl, allowedUpdates: [], cancellationToken: ct);
        return $"Webhook set to {webhookUrl}";
    }

    [HttpPost("bot")] //Handling updates from the bot
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, [FromServices] IUpdateHandler handleUpdateService, CancellationToken ct)
    {
        try
        {
            await handleUpdateService.HandleUpdateAsync(bot, update, ct);
        }
        catch (Exception exception)
        {
            await handleUpdateService.HandleErrorAsync(bot, exception, HandleErrorSource.HandleUpdateError, ct);
        }
        return Ok();
    }
}
