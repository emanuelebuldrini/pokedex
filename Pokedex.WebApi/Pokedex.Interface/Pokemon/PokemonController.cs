using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemon.ApplicationServices;
using PokeDex.Domain;

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
        var pokemon = await pokemonService.GetAsync(name);

        if (pokemon == null)
        {
            return NotFound();
        }

        return Ok(pokemon);
    }
}
