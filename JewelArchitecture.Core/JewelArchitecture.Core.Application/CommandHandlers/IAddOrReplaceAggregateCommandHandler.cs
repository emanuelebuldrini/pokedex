﻿using JewelArchitecture.Core.Application.Commands;
using JewelArchitecture.Core.Domain.Interfaces;

namespace JewelArchitecture.Core.Application.CommandHandlers;

public interface IAddOrReplaceAggregateCommandHandler<TAggregate, TId>
    : IAggregateCommandHandler<TAggregate, TId, AddOrReplaceAggregateCommand<TAggregate, TId>>
    where TAggregate : IAggregateRoot<TId>
    where TId : notnull;