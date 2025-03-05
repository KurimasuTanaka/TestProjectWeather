using System;
using Telegram.Bot;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Repositories;
using WeatherProject.WeatherAPI;

namespace WeatherProject.Bot.Services.MessagesSender;

public class MessagesSender(ITelegramBotClient _telegramBot, IUserRepository _userDataAccess, IWeatherDataReceiver _weatherDataReceiver) : IMessagesSender
{
    public async Task<string> SentMessagesToAll(string? city = null)
    {
        List<User> users = (await _userDataAccess.GetAll()).ToList();

        WeatherData? weatherData = null;
        if (!String.IsNullOrEmpty(city))
        {
            weatherData = await _weatherDataReceiver.GetWeatherData(city);
        }

        foreach (var user in users)
        {
            if (!String.IsNullOrEmpty(city))
            {

                if (weatherData is not null)
                {
                    await _telegramBot.SendMessage(user.id, WeatherDataPrinter.PrintWeatherData(weatherData, city));
                }
                else return "Provided city was not found";

            }
            else

            { //Get weather in chosen city

                if (String.IsNullOrEmpty(user.chosenCity)) continue;
                else
                {
                    WeatherData? chousenCityWeatherData = await _weatherDataReceiver.GetWeatherData(user.chosenCity);

                    if (chousenCityWeatherData is not null)
                    {
                        await _telegramBot.SendMessage(user.id, WeatherDataPrinter.PrintWeatherData(chousenCityWeatherData, user.chosenCity));
                    }
                    else continue;
                }
            }
        }

        return "Messages were sent!";
    }

    public async Task<string> SentMessageToUser(long userId, string? city = null)
    {
        if(String.IsNullOrEmpty(city))
        {
            User user = await _userDataAccess.Get(userId);
            if (String.IsNullOrEmpty(user.chosenCity)) return "City wasn't provided and use didn't have chosen city";
            else
            {
                WeatherData? chousenCityWeatherData = await _weatherDataReceiver.GetWeatherData(user.chosenCity);

                if (chousenCityWeatherData is not null)
                {
                    await _telegramBot.SendMessage(user.id, WeatherDataPrinter.PrintWeatherData(chousenCityWeatherData, user.chosenCity));
                }
                else return "Provided city was not found";
            }
        } else 
        {
            WeatherData? weatherData = await _weatherDataReceiver.GetWeatherData(city);

            if (weatherData is not null)
            {
                await _telegramBot.SendMessage(userId, WeatherDataPrinter.PrintWeatherData(weatherData, city));
            }
            else return "Provided city was not found";
        }

        return "Message was sent!";
    }
}
