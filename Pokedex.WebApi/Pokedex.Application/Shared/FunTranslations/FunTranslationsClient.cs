using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pokedex.Application.Shared.Exceptions;
using Pokedex.Domain.Shared;
using System.Net;
using System.Text.Json;
using UrlCombineLib;

namespace Pokedex.Application.Shared.FunTranslations;

public class FunTranslationsClient : IDisposable
{
    private const string ApiName = "Funtranslations";
    private const string CacheFolder = ".Cache";
    private readonly ILogger<FunTranslationsClient> _logger;
    private readonly HttpClient _httpClient;
    // Use JSON format to get responses from Funtranslations: API default is XML.
    private readonly string _apiResponseFormat = "json";
    private readonly JsonSerializerOptions _jsonOptions;

    public FunTranslationsClient(ILogger<FunTranslationsClient> logger, HttpClient httpClient, IOptions<FunTranslationsApiOptions> options)
    {
        _logger = logger;
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

    public async Task<TDeserialize> TranslateAsync<TDeserialize>(string text, FunTranslation translationType, string cacheId)
        where TDeserialize : class
    {
        var cachedFilePath = Path.Combine(CacheFolder, ApiName, $"{cacheId}.json");

        if (File.Exists(cachedFilePath))
        {
            using var cachedFileStream = new FileStream(cachedFilePath, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<TDeserialize>(cachedFileStream, _jsonOptions) ??
            throw new InvalidDataException($"Unable to deserialize the response to type {nameof(TDeserialize)}");
        }

        var endpoint = translationType.ToString().ToLowerInvariant();
        var queryString = $"text={text}";
        var relativeUri = $"{endpoint}.{_apiResponseFormat}?{queryString}";

        using var response = await _httpClient.GetAsync(relativeUri);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new RateLimitExceededException(ApiName,
                "The rate limit is 60 API calls per day, with a maximum of 5 calls per hour. " +
                "Consider subscribing to a paid plan if you anticipate a higher workload.");
        }

        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();
        // Clone the response stream into a MemoryStream for multiple operations
        using var memoryStream = new MemoryStream();
        await responseStream.CopyToAsync(memoryStream);

        if (!Directory.Exists(CacheFolder))
        {
            Directory.CreateDirectory(CacheFolder);
        }
        var apiCachePath = Path.Combine(CacheFolder, ApiName);
        if (!Directory.Exists(apiCachePath))
        {
            Directory.CreateDirectory(apiCachePath);
        }
        string filePath = Path.Combine(apiCachePath, $"{cacheId}.json");

        try
        {
            // Reset the memory stream position to the beginning
            memoryStream.Position = 0;
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            await memoryStream.CopyToAsync(fileStream);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        memoryStream.Position = 0;
        return await JsonSerializer.DeserializeAsync<TDeserialize>(memoryStream, _jsonOptions) ??
            throw new InvalidDataException($"Unable to deserialize the response to type {nameof(TDeserialize)}");
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}