using JewelArchitecture.Core.Test;
using JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared;

public class PokedexTestBase() : DiTestBase(buildServiceProvider: true)
{
    protected override ServiceCollection GetServiceCollection()
        => ServiceCollectionFactory.GetPokedex();
}
