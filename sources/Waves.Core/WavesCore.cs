using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    /// Gets service provider.
    /// </summary>
    public IWavesServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// Gets service registry.
    /// </summary>
    public IWavesServiceRegistry ServiceRegistry { get; private set; }

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
        _logger.LogDebug($"Container built");
        ServiceProvider = new WavesServiceProvider(_container);
        _logger.LogDebug($"Service provider created");
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task BuildContainerAsync()
    {
        BuildContainer();
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

        if (_logger == null)
        {
            return;
        }

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
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        var configurationFileName = !string.IsNullOrEmpty(env) ? string.Format(Constants.ConfigurationFileName, env) : "appsettings.json";

        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(configurationFileName, optional: true, reloadOnChange: true)
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
        ServiceRegistry = new WavesServiceRegistry(_serviceProvider, _containerBuilder);
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

                await ServiceRegistry.RegisterType(type, registerType, lifetime, key);

                var keyMessage = key != null ? $" with key {key}" : string.Empty;
                _logger.LogDebug(
                    "{Type} registered as {RegisterType} with {Description} lifetime{KeyMessage}",
                    type.GetFriendlyName(),
                    registerType.GetFriendlyName(),
                    lifetime.ToDescription(),
                    keyMessage);
            }
        }
        else
        {
            throw new NullReferenceException("Type loader was not loaded.");
        }
    }
}
