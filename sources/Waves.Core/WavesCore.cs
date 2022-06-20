using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
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
    private Action<IServiceCollection> _configureServices;
    private Action<ILoggingBuilder> _loggingBuilder;

    /// <summary>
    /// Starts core async.
    /// </summary>
    public void Start()
    {
        StartCore();
    }

    /// <summary>
    /// Starts core async.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StartAsync()
    {
        StartCore();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    public void BuildContainer()
    {
        _container = _containerBuilder.Build();
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task BuildContainerAsync()
    {
        _container = _containerBuilder.Build();
        _logger.LogDebug($"Container built");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public T GetInstance<T>(object key = null)
        where T : class
    {
        var result = key == null
            ? _container.Resolve<T>()
            : _container.ResolveKeyed<T>(key);

#if DEBUG
        var stackTrace = new StackTrace(1, false);
        var type = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
        _logger.LogDebug($"{result.GetType().GetFriendlyName()} resolved from container");
#endif

        return result;
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

#if DEBUG
        var stackTrace = new StackTrace(1, false);
        var type = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
        _logger.LogDebug($"{result.GetType().GetFriendlyName()} resolved from container");
#endif

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public object GetInstance(Type type, object key = null)
    {
        var result = key == null
            ? _container.Resolve(type)
            : _container.ResolveKeyed(key, type);

#if DEBUG
        var stackTrace = new StackTrace(1, false);
        var t = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
        _logger.LogDebug($"{result.GetType().GetFriendlyName()} resolved from container");
#endif

        return result;
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

#if DEBUG
        var stackTrace = new StackTrace(1, false);
        var t = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
        _logger.LogDebug($"{result.GetType().GetFriendlyName()} resolved from container");
#endif

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets instances by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public IEnumerable<T> GetInstances<T>(object key = null)
        where T : class
    {
        var results = key == null
            ? _container.Resolve<IEnumerable<T>>()
            : _container.ResolveKeyed<IEnumerable<T>>(key);

        var e = results.ToList();
        foreach (var result in e)
        {
#if DEBUG
            var stackTrace = new StackTrace(1, false);
            var type = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
            _logger.LogDebug($"{result.GetType().GetFriendlyName()} resolved from container");
#endif
        }

        return e.AsEnumerable();
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
#if DEBUG
            var stackTrace = new StackTrace(1, false);
            var type = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
            _logger.LogDebug($"{type.GetFriendlyName()} resolved {result.GetType().GetFriendlyName()} from container");
#endif
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
            _logger.LogError(e, $"Error occured while register instance {obj.GetType().GetFriendlyName()}");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Configures logging.
    /// </summary>
    /// <param name="builder">Logging builder.</param>
    public void AddLogging(Action<ILoggingBuilder> builder)
    {
        _loggingBuilder = builder;
    }

    /// <summary>
    /// Configures services.
    /// </summary>
    /// <param name="configureServices">Configure services action.</param>
    public void AddServices(Action<IServiceCollection> configureServices)
    {
        _configureServices = configureServices;
    }

    /// <summary>
    /// Starts core.
    /// </summary>
    private void StartCore()
    {
        _serviceCollection = new ServiceCollection();

        InitializeServices(_serviceCollection);
        InitializeConfiguration();
        InitializeLogging();
        InitializeServices();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _logger = _serviceProvider.GetService<ILogger<WavesCore>>();

        _logger.LogDebug("Core is starting...");

        InitializeContainer();
        InitializePlugins();

        _logger.LogDebug("Core started");
    }

    /// <summary>
    /// Initializes configuration.
    /// </summary>
    private void InitializeConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(Constants.ConfigurationFileName, optional: true, reloadOnChange: true)
            .Build();
    }

    /// <summary>
    /// Configures logging.
    /// </summary>
    private void InitializeLogging()
    {
        if (_loggingBuilder == null)
        {
            _serviceCollection
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConfiguration(_configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                });
        }
        else
        {
            _serviceCollection.AddLogging(_loggingBuilder);
        }
    }

    /// <summary>
    /// Configures services.
    /// </summary>
    private void InitializeServices(IServiceCollection collection)
    {
        _configureServices?.Invoke(collection);
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
                var key = attribute.Key;
                var lifetime = attribute.Lifetime;

                await RegisterType(type, registerType, lifetime, key);

                var keyMessage = key != null ? $" with key {key}" : string.Empty;
                _logger.LogDebug($"{type.GetFriendlyName()} registered as {registerType.GetFriendlyName()} with {lifetime.ToDescription()} lifetime{keyMessage}");
            }
        }
        else
        {
            throw new NullReferenceException("Type loader was not loaded.");
        }
    }
}
