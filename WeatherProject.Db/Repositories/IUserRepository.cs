using System;
using WeatherProject.Db.DataTransferObjects;

namespace WeatherProject.Db.Repositories;

public interface IUserRepository : IRepository<User, long>
{
    public Task Update(long userId, User user);

}
