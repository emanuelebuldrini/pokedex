﻿using Microsoft.Extensions.DependencyInjection;

namespace JewelArchitecture.Core.Test;

public abstract class DiTestBase : IDisposable
{
    protected readonly ServiceCollection _serviceCollection;
    private ServiceProvider? _serviceProvider;

    protected ServiceProvider? ServiceProvider { get => _serviceProvider; }

    public DiTestBase(bool buildServiceProvider = false)
    {
        _serviceCollection = GetServiceCollection();

        if (buildServiceProvider)
        {
            BuildServiceProvider();
        }
    }

    public void BuildServiceProvider()
    {
        _serviceProvider= _serviceCollection.BuildServiceProvider();
    }

    protected abstract ServiceCollection GetServiceCollection();       

    public virtual void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
