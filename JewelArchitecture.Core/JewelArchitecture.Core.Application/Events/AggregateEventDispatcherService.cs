﻿using JewelArchitecture.Core.Application.Abstractions;
using JewelArchitecture.Core.Domain.Interfaces;

namespace JewelArchitecture.Core.Application;

public class AggregateEventDispatcherService<TAggregate, TId>(IEventDispatcher dispatcher)
    where TAggregate : IAggregateRoot<TId> where TId : notnull 
{
    public async Task DispatchAggregateEventsAsync(TAggregate aggregate)
    {
        foreach (var domainEvent in aggregate.RaisedEvents)
        {
            await DispatchEventAsync(domainEvent);
        }

        // Finally clean up dispatched events.
        aggregate.ClearEvents();
    }

    public async Task DispatchEventAsync(IDomainEvent domainEvent)
    {
        // Resolve the specific domain event type run-time to dispatch it to related event handlers.
        await dispatcher.DispatchAsync((dynamic)domainEvent);
    }
}
