using Pokedex.Domain.Shared;

namespace Pokedex.Application.Shared.FunTranslations;

public class FunTranslationService(FunTranslationsClient funTranslationsClient) : IDisposable
{
    public async Task<string> TranslateAsync(string text, FunTranslation translationType)
    {
        var response = await funTranslationsClient.TranslateAsync<FunTranslationsResponse>(text, translationType);

        return response.Contents.Translated;
    }

    public void Dispose()
    {
        funTranslationsClient.Dispose();
    }
}