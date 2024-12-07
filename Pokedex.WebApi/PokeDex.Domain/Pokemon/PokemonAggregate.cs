﻿using JewelArchitecture.Core.Domain.BaseTypes;
using Pokedex.Domain.Shared;

namespace PokeDex.Domain.Pokemon;

public record PokemonAggregate : AggregateRootBase<int>
{
    public override int Id { get; init; }
    public required string Name { get; init; }
    public required string Habitat { get; init; }
    public required string Description { get; set; }
    public required bool IsLegendary { get; init; }

    public FunTranslation RequiresTranslation()
    {
        if (IsLegendary || Habitat == "cave")
        {
            return FunTranslation.Yoda;
        }
        else
        {
            return FunTranslation.Shakespeare;
        }
    }
}