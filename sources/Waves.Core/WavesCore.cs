using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Waves.Core.Base.Interfaces;
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
        BuildContainer();

        _logger.LogInformation("Core started");

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
        _serviceCollection.AddSingleton<IWavesConfigurationService, WavesConfigurationService>();
        _serviceCollection.AddSingleton<IWavesTypeLoaderService<IWavesPlugin>, WavesTypeLoaderService<IWavesPlugin>>();
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
        var typeLoader = _serviceProvider.GetService<IWavesTypeLoaderService<IWavesPlugin>>();
        if (typeLoader != null)
        {
            await typeLoader.UpdateTypesAsync();
        }
        else
        {
            throw new NullReferenceException("Type loader not loaded.");
        }
    }

    /// <summary>
    /// Builds container.
    /// </summary>
    private void BuildContainer()
    {
        _container = _containerBuilder.Build();
    }
}
