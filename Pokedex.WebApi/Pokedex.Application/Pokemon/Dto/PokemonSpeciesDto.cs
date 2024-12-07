using Pokedex.Application.Shared;

namespace Pokedex.Application.Pokemon.Dto;

public class PokemonSpeciesDto : NameDto
{
    public required NameDto Habitat { get; set; }
    public required bool IsLegendary { get; set; }
    public required FlavorTextEntryDto[] FlavorTextEntries { get; set; }
}