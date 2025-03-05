using System;
using WeatherProject.Db.DataTransferObjects;

namespace WeatherProject.WeatherAPI;

public interface IWeatherDataReceiver
{
    public Task<WeatherData?> GetWeatherData(string city);
}
