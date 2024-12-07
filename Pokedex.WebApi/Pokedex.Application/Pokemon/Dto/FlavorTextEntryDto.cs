namespace Pokedex.Application.Pokemon.Dto;

public class FlavorTextEntryDto
{
    public required string FlavorText { get; set; }
    public required NameDto Language { get; set; }
    public required NameDto Version { get; set; }
}