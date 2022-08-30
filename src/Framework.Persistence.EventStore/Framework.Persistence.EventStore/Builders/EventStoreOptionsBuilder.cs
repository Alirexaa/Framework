
namespace Framework.Persistence.EventStore;

internal sealed class EventStoreOptionsBuilder : IEventStoreOptionsBuilder
{

    private readonly EventStoreOptions _options = new();

    public EventStoreOptions Build()
    {
        return _options;
    }

    public IEventStoreOptionsBuilder WithConnectionString(string connectionString)
    {
        _options.ConnectionString = connectionString;
        return this;
    }
}

