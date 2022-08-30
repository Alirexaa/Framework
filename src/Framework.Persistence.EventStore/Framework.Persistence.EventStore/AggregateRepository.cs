using EventStore.ClientAPI;
using Framework.Abstraction.Persistence.EventStore;
using Framework.Domain.Models;
using Framework.Serialization.Event;
using System.Text;

namespace Framework.Persistence.EventStore;

public class AggregateRepository<TA, TKey> : IAggregateRepository<TA, TKey>
    where TA : class, IAggregateRoot<TKey>
{
    private readonly IEventStoreConnectionWrapper _connectionWrapper;
    private readonly IEventSerializer _eventDeserializer;

    private readonly string _streamBaseName;

    public AggregateRepository(IEventStoreConnectionWrapper connectionWrapper, IEventSerializer eventDeserializer)
    {
        _connectionWrapper = connectionWrapper;

        var aggregateType = typeof(TA);
        _streamBaseName = aggregateType.Name;
        _eventDeserializer = eventDeserializer;
    }

    public async Task PersistAsync(TA aggregateRoot, CancellationToken cancellationToken = default)
    {
        if (null == aggregateRoot)
            throw new ArgumentNullException(nameof(aggregateRoot));

        if (!aggregateRoot.Events.Any())
            return;

        var streamName = GetStreamName(aggregateRoot.Id);

        var firstEvent = aggregateRoot.Events.First();
        var version = firstEvent.AggregateVersion - 1;

        var connection = await _connectionWrapper.GetConnectionAsync();
        using var transaction = await connection.StartTransactionAsync(streamName, version);

        try
        {
            var newEvents = aggregateRoot.Events.Select(Map).ToArray();
            await transaction.WriteAsync(newEvents).ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

        aggregateRoot.ClearEvents();


    }

    public async Task<TA> RehydrateAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionWrapper.GetConnectionAsync();

        var streamName = GetStreamName(key);

        var events = new List<IDomainEvent<TKey>>();

        StreamEventsSlice currentSlice;
        long nextSliceStart = StreamPosition.Start;
        do
        {
            currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);
                                           

            nextSliceStart = currentSlice.NextEventNumber;

            events.AddRange(currentSlice.Events.Select(Map));
        } while (!currentSlice.IsEndOfStream);

        if (!events.Any())
            return null;

        var result = BaseAggregateRoot<TA, TKey>.Create(events.OrderBy(e => e.AggregateVersion));

        return result;
    }


    private IDomainEvent<TKey> Map(ResolvedEvent resolvedEvent)
    {
        var meta = System.Text.Json.JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);
        return _eventDeserializer.Deserialize<TKey>(meta.EventType, resolvedEvent.Event.Data);
    }

    private static EventData Map(IDomainEvent<TKey> @event)
    {
        var json = System.Text.Json.JsonSerializer.Serialize((dynamic)@event);
        var data = Encoding.UTF8.GetBytes(json);

        var eventType = @event.GetType();
        var meta = new EventMeta()
        {
            EventType = eventType.AssemblyQualifiedName
        };
        var metaJson = System.Text.Json.JsonSerializer.Serialize(meta);
        var metadata = Encoding.UTF8.GetBytes(metaJson);

        var eventPayload = new EventData(Guid.NewGuid(), eventType.Name, true, data, metadata);
        return eventPayload;
    }

    



    private string GetStreamName(TKey aggregateKey)
         => $"{_streamBaseName}_{aggregateKey}";


    internal struct EventMeta
    {
        public string EventType { get; set; }
    }
}
