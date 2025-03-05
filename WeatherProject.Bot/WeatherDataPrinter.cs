using System;
using WeatherProject.Db.DataTransferObjects;

namespace WeatherProject.Bot;

//Class for printing weather data in a readable format
public static class WeatherDataPrinter
{
    public static string PrintWeatherData(WeatherData weatherData, string city)
    {
        return
               $"Weather in {city}\n" +
               $"- Description: {weatherData.description} \n" +
               $"- Temperature: {weatherData.temperatureC}°C\n" +
               $"- Temperature: {weatherData.temperatureF}°F\n" +
               $"- HumiFeelsdity: {weatherData.humidity}%\n" +
               $"- Pressure: {weatherData.pressure} hPa\n";
    }
}
