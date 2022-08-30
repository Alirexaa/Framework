using Framework.Domain.Models;

namespace Framework.Abstraction.Persistence.EventStore
{
    public interface IAggregateRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggregateRoot, CancellationToken cancellationToken = default);
        Task<TA> RehydrateAsync(TKey key, CancellationToken cancellationToken = default);
    }
}