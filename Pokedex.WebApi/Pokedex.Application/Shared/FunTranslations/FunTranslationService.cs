using Pokedex.Domain.Shared;

namespace Pokedex.Application.Shared.FunTranslations;

public class FunTranslationService(FunTranslationsClient funTranslationsClient) : IDisposable
{
    // Use JSON format to get responses from Funtranslations: API default is XML.
    private readonly string _apiResponseFormat = "json";

    public async Task<string> TranslateAsync(string text, FunTranslation translationType, string cacheId)
    {
        var endpoint = translationType.ToString().ToLowerInvariant();
        var queryString = $"text={text}";
        var relativeUri = $"{endpoint}.{_apiResponseFormat}?{queryString}";
        var response = await funTranslationsClient.FetchAsync<FunTranslationsResponse>(relativeUri, cacheId);

        return response.Contents.Translated;
    }

    public void Dispose()
    {
        funTranslationsClient.Dispose();
    }
}