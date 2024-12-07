namespace Pokedex.Domain.Pokemon.Exceptions;

public class PokemonNotFoundException(string pokemonName)
    : Exception($"Pokemon with name '{pokemonName}' not found.");
