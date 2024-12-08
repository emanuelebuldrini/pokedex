using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Pokemon.UseCases;
using Pokedex.Application.Shared.Exceptions;
using Pokedex.Domain.Pokemon.Exceptions;
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
    }

    [HttpGet("Translated/{name}")]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        catch (RateLimitExceededException exception)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, exception.Message);
        }
    }
}
