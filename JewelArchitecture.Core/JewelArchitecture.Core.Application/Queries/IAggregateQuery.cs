﻿using JewelArchitecture.Core.Domain.Interfaces;

namespace JewelArchitecture.Core.Application.Queries;

public interface IAggregateQuery<TAggregate, TId> : IQuery
    where TAggregate : IAggregateRoot<TId>
    where TId : notnull 
{
    public TId AggregateId { get; }
}