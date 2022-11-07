using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Enums;
using Waves.Core.Extensions;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Services;

/// <summary>
/// Waves service registry.
/// </summary>
public class WavesServiceRegistry : IWavesServiceRegistry
{
    private readonly ContainerBuilder _containerBuilder;
    private readonly ILogger<WavesServiceRegistry> _logger;

    /// <summary>
    /// Creates new instance of <see cref="WavesServiceRegistry"/>.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="containerBuilder">Container builder.</param>
    public WavesServiceRegistry(
        IServiceProvider serviceProvider,
        ContainerBuilder containerBuilder)
    {
        _containerBuilder = containerBuilder;
        _logger = serviceProvider.GetService<ILogger<WavesServiceRegistry>>();
    }

    /// <summary>
    /// Registers type.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="registerType">Registration type.</param>
    /// <param name="lifetime">Lifetime type.</param>
    /// <param name="key">Register key, may be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task RegisterType(Type type, Type registerType, WavesLifetime lifetime, object key = null)
    {
        try
        {
            switch (lifetime)
            {
                case WavesLifetime.Transient:
                    _containerBuilder.RegisterTransientType(type, registerType, key);
                    break;
                case WavesLifetime.Scoped:
                    _containerBuilder.RegisterScopedType(type, registerType, key);
                    break;
                case WavesLifetime.Singleton:
                    _containerBuilder.RegisterSingletonType(type, registerType, key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while register type {Name}", type.GetFriendlyName());
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Registers instance.
    /// </summary>
    /// <param name="obj">Current object.</param>
    /// <param name="registerType">Registration type.</param>
    /// <param name="lifetime">Lifetime type.</param>
    /// <param name="key">Register key, may be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task RegisterInstance(object obj, Type registerType, WavesLifetime lifetime, object key = null)
    {
        try
        {
            switch (lifetime)
            {
                case WavesLifetime.Transient:
                    _containerBuilder.RegisterTransientInstance(obj, registerType, key);
                    break;
                case WavesLifetime.Scoped:
                    _containerBuilder.RegisterScopedInstance(obj, registerType, key);
                    break;
                case WavesLifetime.Singleton:
                    _containerBuilder.RegisterSingletonInstance(obj, registerType, key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while register instance {Name}", obj.GetType().GetFriendlyName());
        }

        return Task.CompletedTask;
    }
}
