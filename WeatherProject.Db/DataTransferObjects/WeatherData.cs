using System;

namespace WeatherProject.Db.DataTransferObjects;

public class WeatherData
{
    public double temperatureC
    {
        get
        {
            return (temperatureF - 32) * 5 / 9;
        }
        set
        {
            temperatureF = value * 9 / 5 + 32;
        }
    }
    public double temperatureF { get; set; }
    public string description { get; set; } = String.Empty;
    public float humidity { get; set; }
    public float pressure { get; set; }

    public WeatherData(double temperatureF, string description, float humidity, float pressure)
    {
        this.temperatureF = temperatureF;
        this.description = description;
        this.humidity = humidity;
        this.pressure = pressure;
    }
}
