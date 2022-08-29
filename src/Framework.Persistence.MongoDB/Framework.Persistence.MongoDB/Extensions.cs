using Framework.Persistence.MongoDB.Builders;
using Framework.Persistence.MongoDB.Factories;
using Framework.Persistence.MongoDB.Initializers;
using Framework.Persistence.MongoDB.Seaders;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Framework.Abstraction.Types;
using Framework.Persistence.MongoDB.Internals;

namespace Framework.Persistence.MongoDB;

public static class Extensions
{
    // Helpful when dealing with integration testing
    private static bool _conventionsRegistered;
 
    

    public static IServiceCollection AddMongo(this IServiceCollection services, Func<IMongoDbOptionsBuilder,
        IMongoDbOptionsBuilder> buildOptions, Type seederType = null, bool registerConventions = true)
    {
        var mongoOptions = buildOptions(new MongoDbOptionsBuilder()).Build();
        return services.AddMongo(mongoOptions, seederType, registerConventions);
    }

    public static IServiceCollection AddMongo(this IServiceCollection services, MongoDbOptions mongoOptions,
        Type seederType = null, bool registerConventions = true)
    {

        if (mongoOptions.SetRandomDatabaseSuffix)
        {
            var suffix = $"{Guid.NewGuid():N}";
            Console.WriteLine($"Setting a random MongoDB database suffix: '{suffix}'.");
            mongoOptions.Database = $"{mongoOptions.Database}_{suffix}";
        }

        services.AddSingleton(mongoOptions);
        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<MongoDbOptions>();
            return new MongoClient(options.ConnectionString);
        });
        services.AddTransient(sp =>
        {
            var options = sp.GetRequiredService<MongoDbOptions>();
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.Database);
        });
        services.AddTransient<IMongoDbInitializer, MongoDbInitializer>();
        services.AddTransient<IMongoSessionFactory, MongoSessionFactory>();

        if (seederType is null)
        {
            services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();
        }
        else
        {
            services.AddTransient(typeof(IMongoDbSeeder), seederType);
        }

        //services.AddInitializer<IMongoDbInitializer>();
        if (registerConventions && !_conventionsRegistered)
        {
            RegisterConventions();
        }

        return services;
    }

    private static void RegisterConventions()
    {
        _conventionsRegistered = true;
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        BsonSerializer.RegisterSerializer(typeof(decimal?),
            new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
        ConventionRegistry.Register("framework", new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String),
        }, _ => true);
    }

    public static IServiceCollection AddMongoRepository<TEntity,TId>(this IServiceCollection services,
        string collectionName)
        where TEntity : IHaveIdentity<TId>
    {
        services.AddTransient<IMongoRepository<TEntity,TId>>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return new MongoRepository<TEntity, TId>(database, collectionName);
        });

        return services;
    }
}