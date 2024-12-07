using System.ComponentModel.DataAnnotations;

namespace Pokedex.Application.Shared;

public class PokedexApiOptions
{
    [Required]
    public required string Version { get; set; }
    [Required]
    public required string BaseUrl { get; set; }
}
