namespace Pokedex.Domain.Pokemon.Exceptions;

public class PokemonSpeciesNotFoundException(string pokemonSpecies)
    : Exception($"Pokemon species with name '{pokemonSpecies}' not found.");
