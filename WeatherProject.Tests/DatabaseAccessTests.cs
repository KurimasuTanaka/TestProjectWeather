using System;
using Dapper;
using Microsoft.Data.SqlClient;
using WeatherProject.Db.Models;

namespace WeatherProject.Tests;

public class DatabaseAccessTests
{
    [Fact]
    public async Task TestDbConnection()
    {
        var connection = new SqlConnection("Server=localhost;Initial Catalog=WeatherTestProjectDb;Integrated Security=true;TrustServerCertificate=True");
        WeatherHistoryModel? newWeatherHistory = await connection.QueryFirstOrDefaultAsync<WeatherHistoryModel>("SELECT * FROM WeatherHistory");

        // Assert
        Assert.NotNull(newWeatherHistory);
    }
}
