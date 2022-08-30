namespace Framework.Domain.Models;

public interface IAggregateRoot<out TKey> : IEntity<TKey>
{
    long Version { get; }
    IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
    void ClearEvents();
}
