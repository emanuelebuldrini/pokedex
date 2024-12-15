﻿using JewelArchitecture.Core.Test;
using JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared.Factories;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Interface.Shared;
using Pokedex.Test.Shared.Factories;

namespace JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared;

public class PokedexTestBase() : DiTestBase(buildServiceProvider: true)
{
    protected override IServiceCollection GetServiceCollection()
    {
        var config = ConfigurationFactory.GetExternalApiConfig();
        var pokemonApiConfig = config.GetSection("Pokeapi");
        var funTranslationsApiConfig = config.GetSection("FuntranslationsApi");

        var serviceCollection=ServiceCollectionFactory.GetPokedex()
            .AddPokeapiClient(pokemonApiConfig)
            .AddFuntranslationsClient(funTranslationsApiConfig);

        return serviceCollection;
    }
}
