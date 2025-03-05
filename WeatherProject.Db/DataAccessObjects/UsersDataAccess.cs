using System;
using WeatherProject.Db.DataTransferObjects;
using WeatherProject.Db.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using WeatherProject.Db.Models;
using Microsoft.Extensions.Configuration;

namespace WeatherProject.Db.DataAccessObjects;

public class UsersDataAccess(IConfiguration _configuration) : IUserRepository
{
    public async Task Add(User entity)
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);

        await connection.ExecuteAsync("INSERT INTO Users (id, chosenCity) VALUES (@id, @chosenCity)", entity);
    }

    public async Task<User> Get(long id)
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);
        UserModel? newUser = await connection.QueryFirstOrDefaultAsync<UserModel>("SELECT * FROM Users WHERE id = @id", new { id });
    
        if (newUser != null)
        {
            return new User(newUser);
        } else throw new KeyNotFoundException($"User with id {id} not found.");
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);
        List<UserModel> newUser = new(await connection.QueryAsync<UserModel>("SELECT * FROM Users"));
    
        return newUser.Select(user => new User(user));
    }

    public async Task Update(long userId, User user)
    {
        var connection = new SqlConnection(_configuration["SQL:ConnectionString"]);
        
        await connection.ExecuteAsync("UPDATE Users SET chosenCity = @chosenCity WHERE id = @id", user);

    }
}
