using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherProject.Bot.Services.MessagesSender;

namespace WeatherProject.API.Controllers
{
    [Route("/bot")]
    [ApiController]
    public class BotController(IMessagesSender _messagesSender, ILogger<BotController> _logger) : ControllerBase
    {
        [HttpPost("sentWeatherToAll")]
        public async Task<IActionResult> SentWeatherToAll()
        {
            _logger.LogInformation("Sending weather data to all users without a city...");

            string message = await _messagesSender.SentMessagesToAll();

            _logger.LogInformation("Weather data was sent to all users without a city successfully!");

            return Ok(message);

        }

        [HttpPost("sentWeatherToAll/{city}")]
        public async Task<IActionResult> SentWeatherToAll(string city)
        {
            _logger.LogInformation($"Sending weather data about a {city} to all users ...");

            string message = await _messagesSender.SentMessagesToAll(city);

            _logger.LogInformation($"Weather data about a city {city} was sent to all users successfully!");

            return Ok(message);
        }

        [HttpPost("sentWeather/{userId}")]
        public async Task<IActionResult> SentWeather(long userId)
        {
            _logger.LogInformation($"Sending weather data to user with id {userId} ...");

            string message = await _messagesSender.SentMessageToUser(userId);

            _logger.LogInformation($"Weather data was sent to user with id {userId} successfully!");

            return Ok(message);
        }
        [HttpPost("sentWeather/{userId}/{city}")]
        public async Task<IActionResult> SentWeather(long userId, string city)
        {
            _logger.LogInformation($"Sending weather data about a {city} to user with id {userId} ...");

            string message = await _messagesSender.SentMessageToUser(userId, city);

            _logger.LogInformation($"Weather data about a {city} was sent to user with id {userId} successfully!");

            return Ok(message);
        }

    }
}
