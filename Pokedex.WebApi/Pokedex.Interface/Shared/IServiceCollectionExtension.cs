﻿using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Pokemon.UseCases;
using Pokedex.Application.Shared.FunTranslations;
using Pokedex.Application.Shared.Pokeapi;

namespace Pokedex.Interface.Shared;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddPokedex(this IServiceCollection serviceCollection,
        IConfigurationSection pokedexConfigSection,
        IConfigurationSection funTranslationsConfigSection)
    {
        // The following services depend on HttpClient:
        // They should be transient services to avoid trapping HttpClient's lifetime.
        serviceCollection.AddTransient<PokemonService>();
        serviceCollection.AddTransient<PokemonTranslatedCase>();

        serviceCollection.AddTransient<FunTranslationService>();

        // Register external API clients.
        serviceCollection.AddHttpClient<PokeapiClient>();
        serviceCollection.Configure<PokeapiOptions>(pokedexConfigSection)
            .AddOptionsWithValidateOnStart<PokeapiOptions>();

        serviceCollection.AddHttpClient<FunTranslationsClient>();
        serviceCollection.Configure<FunTranslationsApiOptions>(funTranslationsConfigSection)
            .AddOptionsWithValidateOnStart<FunTranslationsApiOptions>();

        return serviceCollection;
    }
}

