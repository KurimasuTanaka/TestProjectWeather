using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace WeatherProject.Bot;

[ApiController]
public class WeatherProjectBotControllers() : ControllerBase
{

    [HttpGet("setWebhook")]
    public async Task<string> SetWebHook([FromServices] ITelegramBotClient bot, CancellationToken ct)
    {
        // var webhookUrl = Config.Value.BotWebhookUrl.AbsoluteUri;
        var webhookUrl = "https://59ec-178-150-31-9.ngrok-free.app/bot";    
        await bot.SetWebhook(webhookUrl, allowedUpdates: [], cancellationToken: ct);
        return $"Webhook set to {webhookUrl}";
    }

    [HttpPost("bot")]
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
