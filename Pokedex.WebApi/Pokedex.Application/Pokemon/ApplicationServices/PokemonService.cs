
using JewelArchitecture.Core.Application.QueryHandlers;
using Microsoft.Extensions.Logging;
using Pokedex.Application.Pokemon.Dto;
using Pokedex.Application.Pokemon.Exceptions;
using Pokedex.Application.Pokemon.Queries;
using Pokedex.Application.Shared;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Application.Pokemon.ApplicationServices;

public class PokemonService(IQueryHandler<PokemonByNameQuery, PokemonDto> pokemonByNameQueryHandler,
     IQueryHandler<PokemonSpeciesByNameQuery, PokemonSpeciesDto> pokemonSpeciesByNameQueryHandler,
     ILogger<PokemonService> logger)
{
    public async Task<PokemonAggregate> GetAsync(string name)
    {
        PokemonDto pokemon;
        PokemonSpeciesDto pokemonSpecies;
        try
        {
            // Get the Pokemon first
            pokemon = await pokemonByNameQueryHandler.HandleAsync(new PokemonByNameQuery(name));
            // Then retrieve the Pokemon species details
            var speciesQuery = new PokemonSpeciesByNameQuery(pokemon.Species.Name);
            pokemonSpecies = await pokemonSpeciesByNameQueryHandler.HandleAsync(speciesQuery);
        }
        catch (HttpRequestException exception)
        {
            var appException = new PokemonDataFetchException(name);
            logger.LogError(exception, appException.Message);

            throw appException;
        }

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
}