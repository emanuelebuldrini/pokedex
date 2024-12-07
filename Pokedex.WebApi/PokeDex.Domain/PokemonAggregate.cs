using JewelArchitecture.Core.Domain.BaseTypes;

namespace PokeDex.Domain;

public record PokemonAggregate : AggregateRootBase<int>
{
    public override int Id { get; init; }
    public required string Name { get; init; }
    public required string Habitat { get; init; }
    public required string Description { get; set; }
    public required bool IsLegendary { get; init; }
}