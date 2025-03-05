using System;

namespace WeatherProject.Db.Models;

public class UserModel
{
    public long id { get; set; }
    public string? chosenCity { get; set; } = String.Empty;

}
