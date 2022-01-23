using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Extensions;
using Waves.Core.Services;
using Waves.Core.Services.Interfaces;

namespace Waves.Core;

/// <summary>
/// Waves core.
/// </summary>
public class WavesCore
{
    private IConfiguration _configuration;
    private IContainer _container;
    private IServiceCollection _serviceCollection;
    private IServiceProvider _serviceProvider;
    private ILogger<WavesCore> _logger;

    private ContainerBuilder _containerBuilder;

    /// <summary>
    /// Starts core async.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StartAsync()
    {
        _serviceCollection = new ServiceCollection();

        InitializeConfiguration();
        InitializeLogging();
        InitializeServices();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _logger = _serviceProvider.GetService<ILogger<WavesCore>>();

        _logger.LogInformation("Core is starting...");

        InitializeContainer();
        InitializePlugins();

        _logger.LogInformation("Core started");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task BuildContainer()
    {
        _container = _containerBuilder.Build();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<T> GetInstanceAsync<T>(object key = null)
        where T : class
    {
        var result = key == null
            ? _container.Resolve<T>()
            : _container.ResolveKeyed<T>(key);

        result.CheckInitializable();

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<object> GetInstanceAsync(Type type, object key = null)
    {
        var result = key == null
            ? _container.Resolve(type)
            : _container.ResolveKeyed(key, type);

        result.CheckInitializable();

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets instances by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<IEnumerable<T>> GetInstancesAsync<T>(object key = null)
        where T : class
    {
        var results = key == null
            ? _container.Resolve<IEnumerable<T>>()
            : _container.ResolveKeyed<IEnumerable<T>>(key);

        var e = results.ToList();
        foreach (var result in e)
        {
            result.CheckInitializable();
        }

        return Task.FromResult(e.AsEnumerable());
    }

    /// <summary>
    /// Registers type.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="registerType">Registration type.</param>
    /// <param name="lifetime">Lifetime type.</param>
    /// <param name="key">Register key, may be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task RegisterType(Type type, Type registerType, WavesLifetimeType lifetime, object key = null)
    {
        try
        {
            switch (lifetime)
            {
                case WavesLifetimeType.Transient:
                    _containerBuilder.RegisterTransientType(type, registerType, key);
                    break;
                case WavesLifetimeType.Scoped:
                    _containerBuilder.RegisterScopedType(type, registerType, key);
                    break;
                case WavesLifetimeType.Singleton:
                    _containerBuilder.RegisterSingletonType(type, registerType, key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occured while register type {type.GetFriendlyName()}");
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
    public Task RegisterInstance(object obj, Type registerType, WavesLifetimeType lifetime, object key = null)
    {
        try
        {
            switch (lifetime)
            {
                case WavesLifetimeType.Transient:
                    _containerBuilder.RegisterTransientInstance(obj, registerType, key);
                    break;
                case WavesLifetimeType.Scoped:
                    _containerBuilder.RegisterScopedInstance(obj, registerType, key);
                    break;
                case WavesLifetimeType.Singleton:
                    _containerBuilder.RegisterSingletonInstance(obj, registerType, key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occured while register instance {obj.GetType().GetFriendlyName()}");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Initializes configuration.
    /// </summary>
    private void InitializeConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.ConfigurationFileName, optional: true, reloadOnChange: true)
            .Build();
    }

    /// <summary>
    /// Configures logging.
    /// </summary>
    private void InitializeLogging()
    {
        _serviceCollection
            .AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(_configuration);
            });
    }

    /// <summary>
    /// Initializes services.
    /// </summary>
    private void InitializeServices()
    {
        _serviceCollection.AddScoped(_ => _configuration);
        _serviceCollection.AddSingleton<IWavesTypeLoaderService<WavesPluginAttribute>, WavesTypeLoaderService<WavesPluginAttribute>>();
        _serviceCollection.AddSingleton(this);
    }

    /// <summary>
    /// Initializes container.
    /// </summary>
    private void InitializeContainer()
    {
        _containerBuilder = new ContainerBuilder();
        _containerBuilder.Populate(_serviceCollection);
    }

    /// <summary>
    /// Initializes plugins.
    /// </summary>
    private async void InitializePlugins()
    {
        var typeLoader = _serviceProvider.GetService<IWavesTypeLoaderService<WavesPluginAttribute>>();
        if (typeLoader != null)
        {
            await typeLoader.UpdateTypesAsync();

            foreach (var pair in typeLoader.Types)
            {
                var attribute = pair.Value;
                var registerType = attribute.Type;
                var type = pair.Key;
                var key = pair.Key;
                var lifetime = attribute.Lifetime;

                await RegisterType(type, registerType, lifetime, key);
            }
        }
        else
        {
            throw new NullReferenceException("Type loader not loaded.");
        }
    }
}
