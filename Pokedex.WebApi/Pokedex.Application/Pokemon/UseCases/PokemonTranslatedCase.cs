
using JewelArchitecture.Core.Application.UseCases;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Shared.FunTranslations;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Application.Pokemon.UseCases
{
    public class PokemonTranslatedCase(PokemonService pokemonService, FunTranslationService funTranslationService)
        : IUseCase<PokemonTranslatedInput, PokemonAggregate>
    {
        public async Task<PokemonAggregate> HandleAsync(PokemonTranslatedInput input)
        {
            var pokemon = await pokemonService.GetAsync(input.PokemonName);

            var translationType = pokemon.RequiresTranslation();
            var translation = await funTranslationService.TranslateAsync(pokemon.Description, translationType);
            
            pokemon.Description = translation;

            return pokemon;
        }
    }
}
