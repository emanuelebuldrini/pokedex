namespace Pokedex.Domain.Exceptions;

public class PokemonNotFoundException(string pokemonName)
    :Exception($"Pokemon with name '{pokemonName}' not found.");
