using System;
using WeatherProject.Db.Models;

namespace WeatherProject.Db.DataTransferObjects;

public class WeatherHistory : WeatherHistoryModel
{
    public WeatherHistory()
    {
    }

    public WeatherHistory(long userId, float temperatureF, DateTime requestDatetime, string city, string description, float humidity, float pressure)
    {
        this.userId = userId;
        this.temperatureF = temperatureF;
        this.requestDatetime = requestDatetime;
        this.city = city;
        this.description = description;
        this.humidity = humidity;
        this.pressure = pressure;
    }
    public WeatherHistory(WeatherHistoryModel weatherHistoryModel)
    {
        this.id = weatherHistoryModel.id;
        this.userId = weatherHistoryModel.userId;
        this.temperatureF = weatherHistoryModel.temperatureF;
        this.requestDatetime = weatherHistoryModel.requestDatetime;
        this.city = weatherHistoryModel.city;
        this.description = weatherHistoryModel.description;
        this.humidity = weatherHistoryModel.humidity;
        this.pressure = weatherHistoryModel.pressure;
    }
}
