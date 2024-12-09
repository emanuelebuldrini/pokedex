using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Pokedex.Application.Shared.Pokeapi;

public sealed class PokeapiClient(HttpClient httpClient, IOptions<PokeapiOptions> options,
    ILogger<PokeapiClient> logger)
    : ApiClient(httpClient, options, logger)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        // Snake case is the default style used by Pokeapi.
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    protected override string ApiName => "Pokeapi";
    protected override JsonSerializerOptions JsonOptions { get => _jsonOptions; }
}