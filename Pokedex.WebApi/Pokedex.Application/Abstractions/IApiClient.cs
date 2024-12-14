namespace Pokedex.Application.Abstractions;

public interface IApiClient
{
    Task<TDeserialize> FetchAsync<TDeserialize>(string relativeUri, string? cacheId)
        where TDeserialize : class;
}
