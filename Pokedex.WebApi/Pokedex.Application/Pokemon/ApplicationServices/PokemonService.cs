
using JewelArchitecture.Core.Application.QueryHandlers;
using Pokedex.Application.Abstractions;
using Pokedex.Application.Pokemon.Dto;
using Pokedex.Application.Pokemon.Queries;
using Pokedex.Application.Shared;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Application.Pokemon.ApplicationServices;

public class PokemonService(IPokeapiClient pokeapiClient,
    IQueryHandler<PokemonByNameQuery,PokemonDto> pokemonByNameQueryHandler)
    : IDisposable
{
    public async Task<PokemonAggregate> GetAsync(string name)
    {
        var pokemon = await pokemonByNameQueryHandler.HandleAsync(new PokemonByNameQuery(name));

        // Details about the Pokemon species is located in a dedicated endpoint.
        var speciesName = pokemon.Species.Name;
        var relativeUri = $"pokemon-species/{speciesName}";
        var pokemonSpecies = await pokeapiClient.FetchAsync<PokemonSpeciesDto>(relativeUri, cacheId: $"{speciesName}.species");

        // Pokemon flavor text comes in different languages and depends on the version of the game.
        var flavorText = pokemonSpecies.FlavorTextEntries
            // By default take the first version of the game in English language, e.g. Red or Blue.
            .FirstOrDefault(e => e.Language.Name == "en")?.FlavorText;

        // Flavor text is returned with common control characters like \n, \f.
        // Therefore, clean up the string.
        var sanitizedFlavorText = Utils.SanitizeFlavorText(flavorText);

        return new PokemonAggregate
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Description = sanitizedFlavorText,
            Habitat = pokemonSpecies.Habitat.Name,
            IsLegendary = pokemonSpecies.IsLegendary
        };
    }

    public void Dispose()
    {
        pokeapiClient.Dispose();
    }
}