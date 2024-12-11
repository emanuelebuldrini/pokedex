using System.ComponentModel.DataAnnotations;

namespace Pokedex.Infrastructure.ApiClients;

public class ApiClientOptions
{
    [Required]
    public required Uri BaseUrl { get; set; }
    public TimeSpan? CacheDuration { get; set; }
}
