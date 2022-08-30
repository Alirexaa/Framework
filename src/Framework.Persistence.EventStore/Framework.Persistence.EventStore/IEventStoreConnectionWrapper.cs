using EventStore.ClientAPI;

namespace Framework.Persistence.EventStore;

public interface IEventStoreConnectionWrapper
{
    Task<IEventStoreConnection> GetConnectionAsync();
}
