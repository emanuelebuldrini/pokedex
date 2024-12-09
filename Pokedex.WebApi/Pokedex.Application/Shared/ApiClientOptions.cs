using System.ComponentModel.DataAnnotations;

namespace Pokedex.Application.Shared;

public class ApiClientOptions
{
    [Required]
    public required Uri BaseUrl { get; set; }
}
