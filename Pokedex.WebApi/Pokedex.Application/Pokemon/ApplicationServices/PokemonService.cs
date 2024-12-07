
using Pokedex.Application.Pokemon.Dto;
using Pokedex.Application.Shared;
using PokeDex.Domain;

namespace Pokedex.Application.Pokemon.ApplicationServices;

public class PokemonService(PokedexClient pokedexClient) : IDisposable
{
    public async Task<PokemonAggregate?> GetAsync(string name)
    {
        var pokemon = await pokedexClient.FetchAsync<PokemonDto>(endpoint: "pokemon", name);
        
        // Details about the Pokemon species is located in a dedicated endpoint.
        var pokemonSpecies = await pokedexClient.FetchAsync<PokemonSpeciesDto>(endpoint: "pokemon-species",
            pokemon.Species.Name);

        // Pokemon flavor text comes in different languages and depends on the version of the game.
        var flavorText = pokemonSpecies.FlavorTextEntries
            // By default take a classic version in English language:
            // Use an entry from the iconic game Pokémon Red for familiarity.
            .FirstOrDefault(e => e.Language.Name == "en" && e.Version.Name == "red")?.FlavorText;

        return new PokemonAggregate
        {
            Id = pokemon.Id,
            Name = pokemon.Name,
            Description = flavorText?? string.Empty,
            Habitat = pokemonSpecies.Habitat.Name,
            IsLegendary = pokemonSpecies.IsLegendary
        };
    }

    public void Dispose()
    {
        pokedexClient.Dispose();
    }
}