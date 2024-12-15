using Pokedex.Application.Abstractions;
using Pokedex.Infrastructure.ApiClients.FunTranslations;

namespace Pokedex.Test.Shared.Fakes;

internal sealed class NastyFuntranslationsClient(FuntranslationsClient client) : IFuntranslationsClient
{
    int retryCount;

    public async Task<TDeserialize> FetchAsync<TDeserialize>(string relativeUri, string? cacheKey)
        where TDeserialize : class
    {
        while (retryCount < 2)
        {
            // It should try at least 2 times.
            retryCount++;
            throw new HttpRequestException("Sorry, unable to reach the Funtranslations API.");
        }

        retryCount = 0;

        return await client.FetchAsync<TDeserialize>(relativeUri, cacheKey);
    }
}