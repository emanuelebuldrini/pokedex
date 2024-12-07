using Microsoft.Extensions.Options;
using System.Text.Json;
using UrlCombineLib;

namespace Pokedex.Application.Shared;

public class PokedexClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _apiVersion;
    private readonly JsonSerializerOptions _jsonOptions;

    public PokedexClient(HttpClient httpClient, IOptions<PokedexApiOptions> options)
    {
        _httpClient = httpClient;
        _apiVersion = options.Value.Version;

        var baseUrl = options.Value.BaseUrl;
        _httpClient.BaseAddress = new Uri(baseUrl.CombineUrl(_apiVersion)
        // Add a final slash to avoid overwriting the base address with the relative address.
            .CombineUrl("/"));

        _jsonOptions = new JsonSerializerOptions
        {
            // Snake case is the default style used by Pokeapi.
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }

    public async Task<TDeserialize> FetchAsync<TDeserialize>(string endpoint, string? resourceName)
        where TDeserialize : class
    {
        using var response = await _httpClient.GetAsync($"{endpoint}/{resourceName}");
        response.EnsureSuccessStatusCode();
        using var stream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<TDeserialize>(stream, _jsonOptions) ??
            throw new InvalidDataException($"Unable to deserialize the response to type {nameof(TDeserialize)}");
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}