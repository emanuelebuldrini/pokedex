namespace Pokedex.Application.Pokemon.Exceptions;

public class PokemonDataFetchException(string pokemonName) 
    :Exception($"Unable to fetch Pokemon data for '{pokemonName}'.");