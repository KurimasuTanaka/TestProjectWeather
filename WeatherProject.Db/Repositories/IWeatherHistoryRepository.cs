using System;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Models;

namespace WeatherProject.Db.Repositories;

public interface IWeatherHistoryRepository : IRepository<WeatherHistory, long>
{
    public Task<IEnumerable<WeatherHistory>> GetAllByUserId(long userId);
}
