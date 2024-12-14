using JewelArchitecture.Core.Application.QueryHandlers;
using Pokedex.Application.Abstractions;
using Pokedex.Application.Pokemon.Dto;
using Pokedex.Application.Pokemon.Queries;
using Pokedex.Domain.Pokemon.Exceptions;
using System.Net;

namespace Pokedex.Application.Pokemon.QueryHandlers;

public class PokemonByNameQueryHandler(IPokeapiClient pokeapiClient)
    : IQueryHandler<PokemonByNameQuery, PokemonDto>
{
    public async Task<PokemonDto> HandleAsync(PokemonByNameQuery query)
    {
        // Pokeapi is case-sensitive and lowercase by default.
        // Make it here case-insensitive to be more user-friendly.
        var pokemonName = query.Name.ToLowerInvariant();
        var relativeUri = $"pokemon/{pokemonName}";

        try
        {
            return await pokeapiClient.FetchAsync<PokemonDto>(relativeUri, cacheId: $"{pokemonName}.pokemon");
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                throw new PokemonNotFoundException(query.Name);
            }
            throw;
        }
    }
}
