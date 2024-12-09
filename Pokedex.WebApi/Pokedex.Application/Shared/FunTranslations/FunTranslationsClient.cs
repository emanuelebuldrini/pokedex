using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Pokedex.Application.Shared.FunTranslations;

public sealed class FunTranslationsClient(HttpClient httpClient, IOptions<FunTranslationsApiOptions> options,
    ILogger<FunTranslationsClient> logger)
    : ApiClient(httpClient, options, logger)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        // Funtranslations API uses a lowercase convention by default.
        PropertyNameCaseInsensitive = true
    };

    protected override string ApiName { get => "Funtranslations"; }

    protected override JsonSerializerOptions JsonOptions { get => _jsonOptions; }
}