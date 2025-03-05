using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using WeatherProject.Db.DataTransferObjects;
using Microsoft.Extensions.Configuration;

namespace WeatherProject.WeatherAPI;

public class WeatherDataReceiver(IConfiguration _configuration) : IWeatherDataReceiver
{
    //Method for getting weather data from the API
    public async Task<WeatherData?> GetWeatherData(string city)
    {
        HttpClient client = new HttpClient();

        string? appid = _configuration["API:AppId"];
        if(appid == null)
        {
            throw new Exception("AppId is not found!!! ");
        }


        HttpResponseMessage response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={appid}&units=imperial");

        if(response.IsSuccessStatusCode)
        {
            JsonNode? node = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync());

            WeatherData weatherData = new WeatherData(
                node!["main"]!["temp"]!.GetValue<float>(),
                node!["weather"]![0]!["description"]!.GetValue<string>(),
                node!["main"]!["humidity"]!.GetValue<float>(),
                node!["main"]!["pressure"]!.GetValue<float>()
            );

            return weatherData;
        }


        return null;
    }
}
