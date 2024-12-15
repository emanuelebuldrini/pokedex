using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Abstractions;
using Pokedex.Application.Shared.FunTranslations;
using Pokedex.Infrastructure.ApiClients.FunTranslations;
using Pokedex.Infrastructure.ApiClients.Pokeapi;
using Pokedex.Test.Shared.Fakes;

namespace Pokedex.Test.Shared;

internal static class IServiceCollectionExtension
{
    // Add external API client helpers for testing purposes.

    public static IServiceCollection AddNastyPokeapiClient(this IServiceCollection serviceCollection,
        IConfigurationSection pokeapiConfigSection)
    {
        serviceCollection.AddTransient<PokeapiClient>();
        serviceCollection.AddTransient<IPokeapiClient, NastyPokeapiClient>();

        serviceCollection.AddHttpClient<NastyPokeapiClient>();
        serviceCollection.Configure<PokeapiOptions>(pokeapiConfigSection)
            .AddOptionsWithValidateOnStart<PokeapiOptions>();

        return serviceCollection;
    }

    public static IServiceCollection AddNastyFuntranslationsClient(this IServiceCollection serviceCollection,
       IConfigurationSection funTranslationsConfigSection)
    {
        serviceCollection.AddTransient<FuntranslationsClient>();
        serviceCollection.AddTransient<IFuntranslationsClient, NastyFuntranslationsClient>();

        serviceCollection.AddHttpClient<FuntranslationsClient>();
        serviceCollection.Configure<FuntranslationsApiOptions>(funTranslationsConfigSection)
            .AddOptionsWithValidateOnStart<FuntranslationsApiOptions>();

        return serviceCollection;
    }
}

