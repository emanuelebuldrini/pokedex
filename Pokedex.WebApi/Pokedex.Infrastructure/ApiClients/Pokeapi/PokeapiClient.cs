using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Application.Abstractions;
using System.Text.Json;

namespace Pokedex.Infrastructure.ApiClients.Pokeapi;

public sealed class PokeapiClient(HttpClient httpClient, IOptions<PokeapiOptions> options,
    ILogger<PokeapiClient> logger)
    : ApiClient(httpClient, options, logger), IPokeapiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        // Snake case is the default style used by Pokeapi.
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    protected override string ApiName => "Pokeapi";
    protected override JsonSerializerOptions JsonOptions { get => _jsonOptions; }
}