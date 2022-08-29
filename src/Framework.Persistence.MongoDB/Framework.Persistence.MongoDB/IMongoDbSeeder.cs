using MongoDB.Driver;

namespace Framework.Persistence.MongoDB;

public interface IMongoDbSeeder
{
    Task SeedAsync(IMongoDatabase database);
}
