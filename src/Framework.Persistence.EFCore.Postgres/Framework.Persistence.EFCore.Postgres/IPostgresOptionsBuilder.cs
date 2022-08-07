namespace Framework.Persistence.EFCore.Postgres;

public interface IPostgresOptionsBuilder
{
    IPostgresOptionsBuilder WithConnectionString(string connectionString);
    IPostgresOptionsBuilder UseInMemoryDatabase(string databaseName);
    PostgresOptions Build();
}
