using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Pokemon.Exceptions;
using Pokedex.Application.Pokemon.UseCases;
using Pokedex.Domain.Pokemon.Exceptions;
using Pokedex.Interface.Shared;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Interface.Pokemon;

[ApiController]
[Route("api/[controller]")]
public class PokemonController(PokemonService pokemonService,
    PokemonTranslatedCase pokemonTranslatedCase) : ControllerBase
{
    [HttpGet("{name}")]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPokemonAsync(string name)
    {
        try
        {
            var pokemon = await pokemonService.GetAsync(name);

            return Ok(pokemon);
        }
        catch (PokemonNotFoundException)
        {
            return NotFound();
        }
        catch (PokemonDataFetchException exception)
        {
            return ApiResponseHelper.InternalServerError(exception);
        }
    }   

    [HttpGet("Translated/{name}")]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPokemonTranslatedAsync(string name)
    {
        try
        {
            var useCaseInput = new PokemonTranslatedInput(name);

            var pokemonTranslated = await pokemonTranslatedCase.HandleAsync(useCaseInput);

            return Ok(pokemonTranslated);
        }
        catch (PokemonNotFoundException)
        {
            return NotFound();
        }
        catch (PokemonDataFetchException exception)
        {
            return ApiResponseHelper.InternalServerError(exception);
        }
    }
}
