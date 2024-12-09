using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlCombineLib;

namespace Pokedex.Application.Shared;

public abstract class ApiClient : IDisposable
{
    private const string CacheFolder = ".Cache";
    private readonly TimeSpan? _cacheDuration;

    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;

    protected abstract string ApiName { get; }

    protected virtual JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions();

    protected ApiClient(HttpClient httpClient, IOptions<ApiClientOptions> options, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cacheDuration = options.Value.CacheDuration;
        _httpClient.BaseAddress = options.Value.BaseUrl
            // Add a final slash to avoid overwriting the base address with the relative address.
            .Combine("/");
    }

    public virtual async Task<TDeserialize> FetchAsync<TDeserialize>(string relativeUri, string? cacheId)
       where TDeserialize : class
    {
        var cachedFilePath = Path.Combine(CacheFolder, ApiName, $"{cacheId}.json");

        if (cacheId != null && File.Exists(cachedFilePath)
            && IsCacheEnabled() && IsCacheExpired(cachedFilePath) == false)
        {
            using var cachedFileStream = new FileStream(cachedFilePath, FileMode.Open, FileAccess.Read);
            return await DeserializeResponse<TDeserialize>(cachedFileStream);
        }

        using var response = await _httpClient.GetAsync(relativeUri);

        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();

        // Clone the response stream into a MemoryStream for multiple operations
        using var memoryStream = new MemoryStream();
        await responseStream.CopyToAsync(memoryStream);

        if (cacheId != null && IsCacheEnabled())
        {
            await CacheResponseAsFile(cacheId, memoryStream);
        }

        memoryStream.Position = 0;
        return await DeserializeResponse<TDeserialize>(memoryStream);
    }

    private bool IsCacheEnabled() => _cacheDuration != null;

    private bool? IsCacheExpired(string cachedFilePath)
    {
        if (_cacheDuration == null)
        {
            return null;
        }

        return DateTime.UtcNow - File.GetCreationTimeUtc(cachedFilePath)
            > _cacheDuration.Value;
    }

    private async Task<TDeserialize> DeserializeResponse<TDeserialize>(Stream stream) where TDeserialize : class
    {
        return await JsonSerializer.DeserializeAsync<TDeserialize>(stream, JsonOptions) ??
            throw new InvalidDataException($"Unable to deserialize the response to type {nameof(TDeserialize)}");
    }

    private async Task CacheResponseAsFile(string cacheId, MemoryStream memoryStream)
    {
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
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
