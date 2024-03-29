using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Attributes;
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
    /// Gets service provider.
    /// </summary>
    internal IWavesServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// Gets service registry.
    /// </summary>
    internal IWavesServiceRegistry ServiceRegistry { get; private set; }

    /// <summary>
    /// Gets configure services action.
    /// </summary>
    internal Action<IServiceCollection> ConfigureServices { get; set; }

    /// <summary>
    /// Gets logging builder action.
    /// </summary>
    internal Action<ILoggingBuilder> LoggingBuilder { get; set; }

    /// <summary>
    /// Starts core.
    /// </summary>
    /// <param name="args">Args.</param>
    public void Start(string[] args = null)
    {
        args ??= Array.Empty<string>();

        _serviceCollection = new ServiceCollection();

        InitializeConfiguration(args);
        InitializeServices(_serviceCollection);
        InitializeLogging();
        InitializeServices();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _logger = _serviceProvider.GetService<ILogger<WavesCore>>();

        if (_logger == null)
        {
            throw new Exception("Logging has not been configured");
        }

        _logger.LogDebug("Core is starting...");

        InitializeContainer();
        InitializePlugins();

        _logger.LogDebug("Core started");
    }

    /// <summary>
    /// Starts core async.
    /// </summary>
    /// <param name="args">Args.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task StartAsync(string[] args = null)
    {
        Start(args);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Configures options.
    /// This method created because of this:
    /// https://github.com/autofac/Autofac/issues/659 .
    /// </summary>
    /// <typeparam name="T">Type of options.</typeparam>
    public void ConfigureOptions<T>()
        where T : class
    {
        _containerBuilder.Register(p => _configuration.Get<T>()).SingleInstance();
    }

    /// <summary>
    /// Configures options.
    /// </summary>
    /// <typeparam name="T">Type of options.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ConfigureOptionsAsync<T>()
        where T : class
    {
        ConfigureOptions<T>();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    /// <returns>Returns container.</returns>
    public IContainer BuildContainer()
    {
        _container = _containerBuilder.Build();
        _logger.LogDebug($"Container built");
        return _container;
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<IContainer> BuildContainerAsync()
    {
        return Task.FromResult(BuildContainer());
    }

    /// <summary>
    /// Initializes configuration.
    /// </summary>
    private void InitializeConfiguration(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        var configurationFileName = !string.IsNullOrEmpty(env) ? string.Format(Constants.ConfigurationFileName, env) : "appsettings.json";

        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(configurationFileName, optional: true, reloadOnChange: true)
            .AddCommandLine(args)
            .AddEnvironmentVariables()
            .Build();
    }

    /// <summary>
    /// Configures logging.
    /// </summary>
    private void InitializeLogging()
    {
        if (LoggingBuilder == null)
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
            _serviceCollection.AddLogging(LoggingBuilder);
        }
    }

    /// <summary>
    /// Configures services.
    /// </summary>
    private void InitializeServices(IServiceCollection collection)
    {
        ConfigureServices?.Invoke(collection);
    }

    /// <summary>
    /// Initializes services.
    /// </summary>
    private void InitializeServices()
    {
        _serviceCollection.AddScoped(_ => _configuration);
        _serviceCollection.AddSingleton<IWavesTypeLoaderService<WavesPluginAttribute>, WavesTypeLoaderService<WavesPluginAttribute>>();
        _serviceCollection.AddSingleton(this);
        _serviceCollection.AddTransient<IWavesServiceProvider>(_ => new WavesServiceProvider(_container));
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
