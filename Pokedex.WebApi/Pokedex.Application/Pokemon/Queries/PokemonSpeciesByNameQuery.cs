using JewelArchitecture.Core.Application.Queries;

namespace Pokedex.Application.Pokemon.Queries;

public record PokemonSpeciesByNameQuery(string Name) : IQuery;