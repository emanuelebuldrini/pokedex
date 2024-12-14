using JewelArchitecture.Core.Application.Queries;

namespace Pokedex.Application.Pokemon.Queries;

public record PokemonByNameQuery(string Name): IQuery;