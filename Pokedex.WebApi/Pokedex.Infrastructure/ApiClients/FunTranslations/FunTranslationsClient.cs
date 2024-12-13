using Microsoft.Extensions.Options;
using Pokedex.Application.Abstractions;
using Pokedex.Application.Shared.FunTranslations;
using Pokedex.Infrastructure.Caching;
using System.Text.Json;

namespace Pokedex.Infrastructure.ApiClients.FunTranslations;

public sealed class FuntranslationsClient(HttpClient httpClient, IOptions<FuntranslationsApiOptions> options,
    IStreamCachingService cachingService)
    : ApiClient(httpClient, options, cachingService), IFuntranslationsClient
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        // Funtranslations API uses a lowercase convention by default.
        PropertyNameCaseInsensitive = true
    };

    protected override string ApiName { get => "Funtranslations"; }

    protected override JsonSerializerOptions JsonOptions { get => _jsonOptions; }
}