using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Pokedex.Infrastructure.ApiClients.Exceptions;
using Pokedex.Infrastructure.Caching;
using UrlCombineLib;

namespace Pokedex.Infrastructure.ApiClients;

public abstract class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IStreamCachingService? _cachingService;
    private readonly TimeSpan? _cacheDuration;

    protected abstract string ApiName { get; }

    protected virtual JsonSerializerOptions JsonOptions { get; } = new JsonSerializerOptions();

    protected ApiClient(HttpClient httpClient, IOptions<ApiClientOptions> options,
        IStreamCachingService? cachingService = null)
    {
        _httpClient = httpClient;
        _cachingService = cachingService;
        _cacheDuration = options.Value.CacheDuration;
        _httpClient.BaseAddress = options.Value.BaseUrl
            // Add a final slash to avoid overwriting the base address with the relative address.
            .Combine("/");
    }

    public virtual async Task<TDeserialize> FetchAsync<TDeserialize>(string relativeUri, string? cacheKey)
       where TDeserialize : class
    {
        if (cacheKey != null && _cacheDuration != null &&
            _cachingService?.TryGetFromCache(cacheTopic: ApiName, cacheKey, _cacheDuration.Value,
            out Stream? cachedResponse) == true)
        {
            var deserializedResponse = await DeserializeResponse<TDeserialize>(cachedResponse!);
            await cachedResponse!.DisposeAsync();

            return deserializedResponse;
        }

        using var response = await _httpClient.GetAsync(relativeUri);
        
        if (ShouldRetryRequest(response))
        {
            throw new HttpRetryableException(response.StatusCode);
        }

        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();

        // Clone the response stream into a MemoryStream for multiple operations
        using var memoryResponseStream = new MemoryStream();
        await responseStream.CopyToAsync(memoryResponseStream);

        if (cacheKey != null && IsCacheEnabled())
        {
            await _cachingService!.CacheResponseAsync(cacheTopic: ApiName, cacheKey, memoryResponseStream);
        }

        memoryResponseStream.Position = 0;
        return await DeserializeResponse<TDeserialize>(memoryResponseStream);
    }

    private static bool ShouldRetryRequest(HttpResponseMessage response)=>
        response.StatusCode != HttpStatusCode.TooManyRequests // Exclude 429
                    // Should retry on 408 or 500                   
                    && (response.StatusCode == HttpStatusCode.RequestTimeout ||
                                       response.StatusCode == HttpStatusCode.InternalServerError);    

    private async Task<TDeserialize> DeserializeResponse<TDeserialize>(Stream stream) where TDeserialize : class
    {
        return await JsonSerializer.DeserializeAsync<TDeserialize>(stream, JsonOptions) ??
            throw new InvalidDataException($"Unable to deserialize the response to type {nameof(TDeserialize)}");
    }

    private bool IsCacheEnabled() => _cachingService != null && _cacheDuration != null;
}
