using System;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.WeatherAPI;

namespace WeatherProject.Tests;

public class DataReceiverTests
{
    [Fact]
    public async Task TestGetWeatherData()
    {
        // Arrange
        string city = "London";
        WeatherDataReceiver weatherDataReceiver = new WeatherDataReceiver();

        // Act
        WeatherData? weatherData = await weatherDataReceiver.GetWeatherData(city);

        // Assert
        Assert.NotEqual(0, weatherData.temperatureC);
        Assert.NotEqual(0, weatherData.temperatureF);
    }
}
