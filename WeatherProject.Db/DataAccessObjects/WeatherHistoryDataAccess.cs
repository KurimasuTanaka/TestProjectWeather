using System;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Models;
using WeatherProject.Db.Repositories;

namespace WeatherProject.Db.DataAccessObjects;

public class WeatherHistoryDataAccess(IConfiguration _configuration) : IWeatherHistoryRepository
{
    public async Task Add(WeatherHistory entity)
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);

        await connection.ExecuteAsync(
            "INSERT INTO WeatherHistory (userId, city, temperatureF, requestDatetime, description, humidity, pressure) VALUES (@userId, @city, @temperatureF, @requestDatetime, @description, @humidity, @pressure)", 
            entity);
    }

    public async Task<WeatherHistory> Get(long id)
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);
        WeatherHistoryModel? newWeatherHistory = await connection.QueryFirstOrDefaultAsync<WeatherHistoryModel>("SELECT * FROM WeatherHistory WHERE id = @id", new { id });
        
        if (newWeatherHistory != null)
        {
            return new WeatherHistory(newWeatherHistory);
        }
        else
        {
            throw new KeyNotFoundException($"WeatherHistory with id {id} not found.");
        }
    }

    public async Task<IEnumerable<WeatherHistory>> GetAll()
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);
        List<WeatherHistoryModel> newWeatherHistory = new(await connection.QueryAsync<WeatherHistoryModel>("SELECT * FROM WeatherHistory"));
    
        return newWeatherHistory.Select(weatherHistory => new WeatherHistory(weatherHistory));
    }

    public  async Task<IEnumerable<WeatherHistory>> GetAllByUserId(long userId)
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);
        List<WeatherHistoryModel> newWeatherHistory = (await connection.QueryAsync<WeatherHistoryModel>("SELECT * FROM WeatherHistory WHERE userId = @userId", new { userId })).ToList();
    
        return newWeatherHistory.Select(weatherHistory => new WeatherHistory(weatherHistory));
    }
}
