using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using UrlCombineLib;

namespace Pokedex.Application.Shared;

public class PokedexClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public PokedexClient(HttpClient httpClient, IOptions<PokedexApiOptions> options)
    {
        _httpClient = httpClient;

        var baseUrl = options.Value.BaseUrl;
        _httpClient.BaseAddress = baseUrl
        // Add a final slash to avoid overwriting the base address with the relative address.
            .Combine("/");

        _jsonOptions = new JsonSerializerOptions
        {
            // Snake case is the default style used by Pokeapi.
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }

    public async Task<TDeserialize?> FetchAsync<TDeserialize>(string endpoint, string? resourceName)
        where TDeserialize : class
    {
        using var response = await _httpClient.GetAsync($"{endpoint}/{resourceName}");
        
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
           return null;
        }  
        
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