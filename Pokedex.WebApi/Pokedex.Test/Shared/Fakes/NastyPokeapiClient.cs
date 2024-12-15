using Pokedex.Application.Abstractions;
using Pokedex.Infrastructure.ApiClients.Pokeapi;

namespace Pokedex.Test.Shared.Fakes;

public class NastyPokeapiClient(PokeapiClient client) : IPokeapiClient
{
    int retryCount;

    public async Task<TDeserialize> FetchAsync<TDeserialize>(string relativeUri, string? cacheKey)
        where TDeserialize : class
    {
        while (retryCount < 3)
        {
            // It should try at least 3 times.
            retryCount++;
            throw new HttpRequestException("Sorry, unable to reach the API.");
        }

        retryCount = 0;

        return await client.FetchAsync<TDeserialize>(relativeUri, cacheKey);
    }
}