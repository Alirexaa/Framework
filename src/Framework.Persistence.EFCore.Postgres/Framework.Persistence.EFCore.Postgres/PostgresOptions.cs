namespace Framework.Persistence.EFCore.Postgres;

public class PostgresOptions
{
    public string ConnectionString { get; set; } = default!;
    public string InMemoryDatabaseName { get; set; } = "InMemory";
    public bool UseInMemoryDatabase { get; set; }
}
