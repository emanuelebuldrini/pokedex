﻿using JewelArchitecture.Core.Test;
using JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared.Factories;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Interface.Shared;
using Pokedex.Test.Shared;
using Pokedex.Test.Shared.Factories;

namespace JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared;

public class PokedexResilienceTestBase() : DiTestBase(buildServiceProvider: true)
{
    protected override IServiceCollection GetServiceCollection()
    {
        var config = ConfigurationFactory.GetExternalApiConfig();
        var pokemonApiConfig = config.GetSection("Pokeapi");
        var funTranslationsApiConfig = config.GetSection("FuntranslationsApi");
        var retryPolicyConfig = config.GetSection("RetryPolicy");

        var serviceCollection = ServiceCollectionFactory.GetPokedex()
            .AddNastyPokeapiClient(pokemonApiConfig)
            .AddNastyFuntranslationsClient(funTranslationsApiConfig)
            .AddRetryPolicyOptions(retryPolicyConfig);

        return serviceCollection;
    }
}
