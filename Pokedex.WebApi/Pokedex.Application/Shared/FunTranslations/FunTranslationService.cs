using Pokedex.Domain.Shared;

namespace Pokedex.Application.Shared.FunTranslations;

public class FunTranslationService(FunTranslationsClient funTranslationsClient) : IDisposable
{
    public async Task<string> TranslateAsync(string text, FunTranslation translationType, string cacheId)
    {
        var response = await funTranslationsClient.TranslateAsync<FunTranslationsResponse>(text, translationType, cacheId);

        return response.Contents.Translated;
    }

    public void Dispose()
    {
        funTranslationsClient.Dispose();
    }
}