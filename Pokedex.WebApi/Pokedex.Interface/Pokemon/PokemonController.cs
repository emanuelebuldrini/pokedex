using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Pokemon.Exceptions;
using Pokedex.Application.Pokemon.UseCases;
using Pokedex.Domain.Pokemon.Exceptions;
using Pokedex.Interface.Shared;
using PokeDex.Domain.Pokemon;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.Interface.Pokemon;

[ApiController]
[Route("api/[controller]")]
public class PokemonController(PokemonService pokemonService,
    PokemonTranslatedCase pokemonTranslatedCase) : ControllerBase
{
    /// <summary>
    /// Get a Pok�mon by name.
    /// </summary>
    /// <param name="name">The name of the Pok�mon.</param>
    /// <response code="200">Returns the Pok�mon details successfully.</response>
    /// <response code="404">If the Pok�mon is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("{name}")]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPokemonAsync([MaxLength(32)][Required] string name)
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

    /// <summary>
    /// Get a Pok�mon by name with its description translated.
    /// </summary>
    /// <remarks>
    /// Catch a Pok�mon with a cave habitat or legendary status to receive the Yoda translation.
    /// For all others, the Shakespeare translation will be applied.
    /// Beware, young trainer! This API is rate-limited�too many requests, and you might exhaust the patience of the server.
    /// If that happens, the standard Pok�mon description will be your consolation prize.
    /// </remarks>
    /// <param name="name">The name of the Pok�mon.</param>
    /// <response code="200">Returns the Pok�mon details with a translated description.</response>
    /// <response code="404">If the Pok�mon is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("Translated/{name}")]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPokemonTranslatedAsync([MaxLength(32)][Required] string name)
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
