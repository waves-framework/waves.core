using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Waves.Core.Services;
using Waves.Core.Services.Interfaces;

namespace Waves.Core;

/// <summary>
/// Waves core.
/// </summary>
public class WavesCore
{
    private IConfiguration _configuration;
    private IServiceCollection _serviceCollection;
    private ILogger<WavesCore> _logger;

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

        var provider = _serviceCollection.BuildServiceProvider();

        _logger = provider.GetService<ILogger<WavesCore>>();
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
        _serviceCollection.AddSingleton(this);
        _serviceCollection.AddSingleton<IWavesConfigurationService, WavesConfigurationService>();
    }
}
