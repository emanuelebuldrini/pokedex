
using Pokedex.Application.Pokemon.Dto;
using Pokedex.Application.Shared;
using Pokedex.Application.Shared.Pokeapi;
using Pokedex.Domain.Pokemon.Exceptions;
using PokeDex.Domain.Pokemon;
using System.Net;

namespace Pokedex.Application.Pokemon.ApplicationServices;

public class PokemonService(PokeapiClient pokeapiClient) : IDisposable
{
    public async Task<PokemonAggregate> GetAsync(string name)
    {
        // Pokeapi is case-sensitive and lowercase by default.
        // Make it here case-insensitive to be more user-friendly.
        var pokemonName = name.ToLowerInvariant();
        var relativeUri = $"pokemon/{pokemonName}";

        PokemonDto pokemon;
        try
        {
            pokemon = await pokeapiClient.FetchAsync<PokemonDto>(relativeUri, cacheId: $"{pokemonName}.pokemon");
        }
        catch (HttpRequestException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                throw new PokemonNotFoundException(name);
            }
            throw;
        }

        // Details about the Pokemon species is located in a dedicated endpoint.
        var speciesName = pokemon.Species.Name;
        relativeUri = $"pokemon-species/{speciesName}";
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