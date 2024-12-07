using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Domain.Exceptions;
using PokeDex.Domain.Pokemon;

namespace Pokedex.Interface.Pokemon;

[ApiController]
[Route("api/[controller]")]
public class PokemonController(PokemonService pokemonService) : ControllerBase
{
    [HttpGet("{name}")]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status200OK)]
    [ProducesResponseType<PokemonAggregate>(StatusCodes.Status404NotFound)]
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
}
