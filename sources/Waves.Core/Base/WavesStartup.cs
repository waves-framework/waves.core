using System;
using System.Linq;
using AspNetCore.AsyncInitialization;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Services;

namespace Waves.Core.Base;

/// <summary>
/// Abstract waves framework startup class.
/// </summary>
public abstract class WavesStartup : IWavesStartup
{
    /// <inheritdoc />
    public void ConfigureServices(IServiceCollection services)
    {
        // register plugins.
        RegisterPlugins(services);

        // add logging.
        services.AddLogging();

        // run virtual method for registering additional services.
        ConfigureAdditionalServices(services);
    }

    /// <summary>
    /// Configures additional services.
    /// </summary>
    /// <param name="services">Services collection.</param>
    protected virtual void ConfigureAdditionalServices(IServiceCollection services)
    {
        // TODO: add your services here.
    }

    /// <summary>
    /// Registers plugins with type loader service.
    /// </summary>
    /// <param name="services">Service collection.</param>
    private static async void RegisterPlugins(IServiceCollection services)
    {
        var typeLoader = new WavesTypeLoaderService<WavesPluginAttribute>();
        await typeLoader.UpdateTypesAsync();
        var types = typeLoader.Types;

        foreach (var type in types)
        {
            var attribute = type.Value;

            if (type.Key.GetInterfaces().Contains(typeof(IAsyncInitializer)))
            {
                services.AddAsyncInitializer(type.Key);
                continue;
            }

            switch (attribute.Lifetime)
            {
                case WavesLifetimeType.Transient:
                    services.AddTransient(attribute.Type, type.Key);
                    break;
                case WavesLifetimeType.Scoped:
                    services.AddScoped(attribute.Type, type.Key);
                    break;
                case WavesLifetimeType.Singleton:
                    services.AddSingleton(attribute.Type, type.Key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
