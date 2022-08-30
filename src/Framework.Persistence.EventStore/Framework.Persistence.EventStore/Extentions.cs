using Framework.Abstraction.Persistence.EventStore;
using Framework.Domain.Models;
using Framework.Serialization.Event;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistence.EventStore
{
    public static class Extentions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, string connectionString)
        {
            return services.AddSingleton<IEventStoreConnectionWrapper>(ctx =>
            {
                var logger = ctx.GetRequiredService<ILogger<EventStoreConnectionWrapper>>();
                return new EventStoreConnectionWrapper(new Uri(connectionString), logger);
            });
        }

        public static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IAggregateRepository<TA, TK>>(ctx =>
            {
                var connectionWrapper = ctx.GetRequiredService<IEventStoreConnectionWrapper>();
                var eventDeserializer = ctx.GetRequiredService<IEventSerializer>();
                return new AggregateRepository<TA, TK>(connectionWrapper, eventDeserializer);
            });
        }
    }
}
