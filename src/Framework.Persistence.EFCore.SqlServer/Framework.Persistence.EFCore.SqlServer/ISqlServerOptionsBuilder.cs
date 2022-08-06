namespace Framework.Persistence.EFCore.SqlServer;

public interface ISqlServerOptionsBuilder
{
    ISqlServerOptionsBuilder WithConnectionString(string connectionString);
    ISqlServerOptionsBuilder UseInMemoryDatabase(string databaseName);
    SqlServerOptions Build();
}


