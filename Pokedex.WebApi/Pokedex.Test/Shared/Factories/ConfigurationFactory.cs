using Microsoft.Extensions.Configuration;

namespace Pokedex.Test.Shared.Factories;

public class ConfigurationFactory
{
    public static IConfigurationRoot GetExternalApiConfig()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "PokedexApi:BaseUrl", "https://pokeapi.co/api/v2" },
            { "PokedexApi:CacheDuration", "10675199.02:48:05" },
            { "FunTranslationsApi:BaseUrl", "https://api.funtranslations.com/translate" },
            // Set essentially no cache expiration for testing purposes: duration of TimeSpan.MaxValue.
            { "FunTranslationsApi:CacheDuration", "10675199.02:48:05" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
}
