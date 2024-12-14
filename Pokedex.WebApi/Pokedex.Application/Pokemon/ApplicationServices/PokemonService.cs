
using JewelArchitecture.Core.Application.QueryHandlers;
using Pokedex.Application.Pokemon.Dto;
using Pokedex.Application.Pokemon.Queries;
using Pokedex.Application.Shared;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Application.Pokemon.ApplicationServices;

public class PokemonService(IQueryHandler<PokemonByNameQuery,PokemonDto> pokemonByNameQueryHandler,
     IQueryHandler<PokemonSpeciesByNameQuery, PokemonSpeciesDto> pokemonSpeciesByNameQueryHandler)   
{
    public async Task<PokemonAggregate> GetAsync(string name)
    {
        // Get the Pokemon first
        var pokemon = await pokemonByNameQueryHandler.HandleAsync(new PokemonByNameQuery(name));
        // Then retrieve the Pokemon species details
        var speciesQuery = new PokemonSpeciesByNameQuery(pokemon.Species.Name);
        var pokemonSpecies = await pokemonSpeciesByNameQueryHandler.HandleAsync(speciesQuery);

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