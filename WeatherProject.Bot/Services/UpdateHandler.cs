
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherProject.Bot;
using DTO = WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Repositories;
using WeatherProject.WeatherAPI;
using WeatherProject.Db.DataTransferObjects;

public class UpdateHandler(
    ITelegramBotClient _botClient,
    ILogger<UpdateHandler> _logger,
    IWeatherDataReceiver _weatherDataReceiver,
    IUserRepository _userDataAccess,
    IWeatherHistoryRepository _weatherHistoryDataAccess) : IUpdateHandler
{
    //Error handler for the bot
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        _logger.LogError("Handle error: {error}", exception); // just dump the exception to the console

        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (update.Type != UpdateType.Message)
        {
            return;
        }
        await OnMessage(update.Message!);
    }

    //Method for handling messages
    public async Task OnMessage(Message message)
    {
        if (message.Text is not { } messageText)
            return;


        _logger.LogInformation("Message was received. Processing...");


        if (messageText.StartsWith("/weather") || messageText.StartsWith("Get weather in")) await SentWeatherData(message);
        else if (messageText.StartsWith("/chooseCity")) await ChooseCity(message);
        else if (messageText.StartsWith("/start")) await SentStartingMessage(message);
        else await UnknownCommandReceiver(message);

    }

    //Method that returns a button with the city that the user has chosen. If the city isn't chosen, the button is NULL
    private async Task<KeyboardButton?> GetCityWeatherButton(long userId)
    {
        KeyboardButton? cityButton;

        DTO.User user = await _userDataAccess.Get(userId);
        if (user == null || String.IsNullOrEmpty(user.chosenCity))
        {
            cityButton = null;
        }
        else
        {
            cityButton = new KeyboardButton("Get weather in " + user.chosenCity);
        }
        return cityButton;
    }

    //Method for handling messages with unknown commands
    private async Task UnknownCommandReceiver(Message message)
    {
        _logger.LogInformation("Unknown command was received! Sending the unknown command message...");

        await _botClient.SendMessage(message.Chat.Id, "Unknown command!", replyMarkup: await GetCityWeatherButton(message.Chat.Id));
    
        _logger.LogInformation("Unknown command message was sent successfully!");
    }

    //Method for sending the starting message
    private async Task SentStartingMessage(Message message)
    {
        _logger.LogInformation("Starting message was received! Sending the starting message...");

        await _botClient.SendMessage(
            message.Chat.Id,
            @"/weather - Get weather in your chosen city
            /chooseCity - Choose a city to get weather by pressing the button
            ",
            replyMarkup: await GetCityWeatherButton(message.Chat.Id));
    
        _logger.LogInformation("Starting message was sent successfully!");
    }

    //Method for choosing a city. If user wan't in DB then the new User instance is created. 
    //If the user was in DB then the city is updated
    private async Task ChooseCity(Message message)
    {
        _logger.LogInformation("City choosing message was received! Processing...");

        string chosenCity = message.Text!.Split(" ").Last();

        _logger.LogInformation($"City {chosenCity} was chosen by user {message.Chat.Id} ! Saving to the database...");

        DTO.User user = await _userDataAccess.Get(message.Chat.Id);
        if (user == null)
        {
            user = new DTO.User(message.Chat.Id, chosenCity); // Add a default value if null
            await _userDataAccess.Add(user);
        }
        else
        {
            user.chosenCity = chosenCity;

            await _userDataAccess.Update(message.Chat.Id, user);

        }

        _logger.LogInformation("City was chosen and saved to the database successfully! Sending the confirmation message...");

        await _botClient.SendMessage(message.Chat.Id, "City was chosen", replyMarkup: await GetCityWeatherButton(message.Chat.Id));
    
        _logger.LogInformation("Confirmation message was sent successfully!");
    }


    //Method for sending weather data
    //Information about all requests is saved to the database
    private async Task SentWeatherData(Message message)
    {
        _logger.LogInformation("Weather data request was received! Processing...");

        string cityToCheck = message.Text!.Split(" ").Last();

        _logger.LogInformation($"User {message.Chat.Id} requested weather in {cityToCheck}! Getting weather data...");


        WeatherData? weatherData = await _weatherDataReceiver.GetWeatherData(cityToCheck);

        if (weatherData is null)
        {
            _logger.LogInformation("Provided city was not found! Sending the error message...");

            await _botClient.SendMessage(message.Chat.Id, "Provided city was not found", replyMarkup: await GetCityWeatherButton(message.Chat.Id));
            return;
        }

        _logger.LogInformation("Weather data was received! Saving to the database...");

        DTO.User user = await _userDataAccess.Get(message.Chat.Id);
        if (user == null)
        {
            user = new DTO.User(message.Chat.Id, null); // Add a default value if null
            await _userDataAccess.Add(user);
        }
        await _weatherHistoryDataAccess.Add(new WeatherHistory(
            user.id, (float)weatherData.temperatureF, DateTime.Now, cityToCheck, weatherData.description, weatherData.humidity, weatherData.pressure));

        _logger.LogInformation("Weather data was saved to the database successfully! Sending the weather data...");

        await _botClient.SendMessage(
            message.Chat.Id,
            WeatherDataPrinter.PrintWeatherData(weatherData, cityToCheck),
            replyMarkup: await GetCityWeatherButton(message.Chat.Id));

        _logger.LogInformation("Weather data was sent successfully!");
    }
}