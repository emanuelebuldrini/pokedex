
using JewelArchitecture.Core.Application.UseCases;
using Microsoft.Extensions.Logging;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Shared.FunTranslations;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Application.Pokemon.UseCases
{
    public class PokemonTranslatedCase(PokemonService pokemonService, FunTranslationService funTranslationService,
        ILogger<PokemonTranslatedCase> logger)
        : IUseCase<PokemonTranslatedInput, PokemonAggregate>
    {
        public async Task<PokemonAggregate> HandleAsync(PokemonTranslatedInput input)
        {
            var pokemon = await pokemonService.GetAsync(input.PokemonName);

            var translationType = pokemon.RequiresTranslation();
            try
            {
                var translation = await funTranslationService.TranslateAsync(pokemon.Description, translationType);
                pokemon.Description = translation;
            }
            catch (Exception exception)
            {
                // It returns the Pokemon with the standard description if the translation failed for any reason.
                logger.LogError(exception.Message);
            }

            return pokemon;
        }
    }
}
