
using JewelArchitecture.Core.Application.QueryHandlers;
using JewelArchitecture.Core.Application.UseCases;
using Microsoft.Extensions.Logging;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Shared;
using Pokedex.Application.Shared.FunTranslations;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Application.Pokemon.UseCases
{
    public class PokemonTranslatedCase(PokemonService pokemonService,
        IQueryHandler<FunTranslationQueryByText, string> funTranslationQueryByTextHandler,
        ILogger<PokemonTranslatedCase> logger)
        : IUseCase<PokemonTranslatedInput, PokemonAggregate>
    {
        public async Task<PokemonAggregate> HandleAsync(PokemonTranslatedInput input)
        {
            var pokemon = await pokemonService.GetAsync(input.PokemonName);
            var translationType = pokemon.RequiresTranslation();
            try
            {
                // Cache translation by Pokemon.
                var cacheKey = $"{pokemon.Name}.description";
                var query = new FunTranslationQueryByText(pokemon.Description, translationType, cacheKey);
                var translation = await funTranslationQueryByTextHandler.HandleAsync(query);

                // Yoda translation response needs sanitization because has double spaces after comma.
                // Furthermore, if there is a full stop between two sentences it has no space after.
                // And sometimes it has commas without a space after.
                // And the first letter after comma is from time to time capitalized.
                pokemon.Description = Utils.SanitizeTranslation(translation);
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
