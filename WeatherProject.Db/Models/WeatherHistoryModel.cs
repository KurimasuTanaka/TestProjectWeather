using System;

namespace WeatherProject.Db.Models;

public class WeatherHistoryModel
{
    public long id { get; set; }
    public long userId { get; set; }
    public string city { get; set; } = String.Empty;
    public float temperatureF { get; set; }
    public DateTime requestDatetime { get; set; }
    public string description { get; set; } = String.Empty;
    public float humidity { get; set; }
    public float pressure { get; set; }
}

