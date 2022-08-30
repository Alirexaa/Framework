namespace Framework.Persistence.EventStore;

public interface IEventStoreOptionsBuilder
{
    IEventStoreOptionsBuilder WithConnectionString(string connectionString);
    EventStoreOptions Build();
}
