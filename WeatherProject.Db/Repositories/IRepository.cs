using System;

namespace WeatherProject.Db.Repositories;

public interface IRepository<EntityT, KeyT>
{
    public Task<EntityT> Get(KeyT id);
    public Task<IEnumerable<EntityT>> GetAll();
    public Task Add(EntityT entity);
}
