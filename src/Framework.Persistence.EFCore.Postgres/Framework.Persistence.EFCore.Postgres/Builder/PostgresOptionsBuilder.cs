
namespace Framework.Persistence.EFCore.Postgres.Builder;

internal sealed class PostgresOptionsBuilder : IPostgresOptionsBuilder
{

    private readonly PostgresOptions _options = new();

    public PostgresOptions Build()
    {
        return _options;
    }

    public IPostgresOptionsBuilder UseInMemoryDatabase(string databaseName)
    {
        _options.UseInMemoryDatabase = true;
        _options.InMemoryDatabaseName = databaseName;
        return this;
    }

    public IPostgresOptionsBuilder WithConnectionString(string connectionString)
    {
        _options.ConnectionString = connectionString;
        return this;
    }

}

