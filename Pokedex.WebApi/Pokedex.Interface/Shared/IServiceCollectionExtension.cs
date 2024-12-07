using Pokedex.Application.Pokemon.ApplicationServices;
using Pokedex.Application.Shared;

namespace Pokedex.Interface.Shared;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddPokedex(this IServiceCollection serviceCollection,
        IConfigurationSection pokedexConfigSection)
    {
        // It depends on HttpClient: It should be a transient service to avoid trapping HttpClient's lifetime.
        serviceCollection.AddTransient<PokemonService>();

        serviceCollection.AddHttpClient<PokedexClient>();
        serviceCollection.Configure<PokedexApiOptions>(pokedexConfigSection)
            .AddOptionsWithValidateOnStart<PokedexApiOptions>();

        return serviceCollection;
    }
}

