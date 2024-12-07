using JewelArchitecture.Core.Interface.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Interface.Pokemon;
using Pokedex.Interface.Shared;
using System.Reflection;

namespace JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared.Factories;

internal class ServiceCollectionFactory
{
    public static ServiceCollection GetPokedex()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddControllers()
            .AddApplicationPart(Assembly.GetAssembly(typeof(PokemonController))!)
            .AddControllersAsServices();

        var configuration = GetPokemonApiConfig();

        serviceCollection
            .AddJewelArchitecture()
            .AddPokedex(configuration.GetSection("PokedexApi"));

        return serviceCollection;
    }

    private static IConfigurationRoot GetPokemonApiConfig()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "PokedexApi:BaseUrl", "https://pokeapi.co/api" },
            { "PokedexApi:Version", "v2" },
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
}
