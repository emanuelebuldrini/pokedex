using Microsoft.Extensions.Options;
using Pokedex.Application.Shared.Exceptions;
using Pokedex.Domain.Shared;
using System.Net;
using System.Text.Json;
using UrlCombineLib;

namespace Pokedex.Application.Shared.FunTranslations;

public class FunTranslationsClient : IDisposable
{
    private readonly HttpClient _httpClient;
    // Use JSON format to get responses from Funtranslations: API default is XML.
    private readonly string _apiResponseFormat = "json";
    private readonly JsonSerializerOptions _jsonOptions;

    public FunTranslationsClient(HttpClient httpClient, IOptions<FunTranslationsApiOptions> options)
    {
        _httpClient = httpClient;

        var baseUrl = options.Value.BaseUrl;

        // Add a final slash to avoid overwriting the base address with the relative address.
        _httpClient.BaseAddress = new Uri(baseUrl.CombineUrl("/"));

        _jsonOptions = new JsonSerializerOptions
        {
            // Funtranslations API uses a lowercase convention by default.
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<TDeserialize> TranslateAsync<TDeserialize>(string text, FunTranslation translationType)
        where TDeserialize : class
    {
        var endpoint = translationType.ToString().ToLowerInvariant();
        var queryString = $"text={text}";
        var relativeUri = $"{endpoint}.{_apiResponseFormat}?{queryString}";

        using var response = await _httpClient.GetAsync(relativeUri);
        
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new RateLimitExceededException("Funtranslations",
                "The rate limit is 60 API calls per day, with a maximum of 5 calls per hour. " +
                "Consider subscribing to a paid plan if you anticipate a higher workload.");
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