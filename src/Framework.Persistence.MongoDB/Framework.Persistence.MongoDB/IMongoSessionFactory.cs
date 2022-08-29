using MongoDB.Driver;

namespace Framework.Persistence.MongoDB;

public interface IMongoSessionFactory
{
    Task<IClientSessionHandle> CreateAsync();
}