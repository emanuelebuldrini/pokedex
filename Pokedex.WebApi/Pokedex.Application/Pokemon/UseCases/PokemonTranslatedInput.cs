using JewelArchitecture.Core.Application.UseCases;

namespace Pokedex.Application.Pokemon.UseCases
{
    public record PokemonTranslatedInput(string PokemonName) : IUseCaseInput;
}