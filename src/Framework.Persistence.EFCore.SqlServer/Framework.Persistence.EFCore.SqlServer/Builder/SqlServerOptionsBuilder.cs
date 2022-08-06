namespace Framework.Persistence.EFCore.SqlServer.Builder;

internal sealed class SqlServerOptionsBuilder : ISqlServerOptionsBuilder
{

    private readonly SqlServerOptions _options = new();

    public SqlServerOptions Build()
    {
        return _options;
    }

    public ISqlServerOptionsBuilder UseInMemoryDatabase(string databaseName)
    {
        _options.UseInMemoryDatabase = true;
        _options.InMemoryDatabaseName = databaseName;
        return this;
    }

    public ISqlServerOptionsBuilder WithConnectionString(string connectionString)
    {
        _options.ConnectionString = connectionString;
        return this;
    }
 
}

