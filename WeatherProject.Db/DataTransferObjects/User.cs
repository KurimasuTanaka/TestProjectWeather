using System;
using WeatherProject.Db.Models;

namespace WeatherProject.Db.DataTransferObjects;

public class User : UserModel
{
    public User()
    {
    }

    public User(long id, string? chosenCity)
    {
        this.id = id;
        this.chosenCity = chosenCity;
    }

    public User(UserModel userModel)
    {
        this.id = userModel.id;
        this.chosenCity = userModel.chosenCity;
    }
}
