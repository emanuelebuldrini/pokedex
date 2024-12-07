using System.ComponentModel.DataAnnotations;

namespace Pokedex.Application.Shared.FunTranslations;

public class FunTranslationsApiOptions
{
    [Required]
    public required string BaseUrl { get; set; }
}
