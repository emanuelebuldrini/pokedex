using Microsoft.Extensions.Configuration;

namespace Pokedex.Test.Shared.Factories;

public class ConfigurationFactory
{
    public static IConfigurationRoot GetExternalApiConfig()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "PokedexApi:BaseUrl", "https://pokeapi.co/api/v2" },
            { "FunTranslationsApi:BaseUrl", "https://api.funtranslations.com/translate" }
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
}
