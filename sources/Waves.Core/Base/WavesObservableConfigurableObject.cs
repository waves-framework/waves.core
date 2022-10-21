using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;
using Waves.Core.Extensions;

namespace Waves.Core.Base;

/// <summary>
/// Abstraction for observable / configurable objects.
/// </summary>
public abstract class WavesObservableConfigurableObject :
    WavesObservableInitializableObject,
    IWavesObservableConfigurableObject
{
    private readonly Dictionary<string, string> _configurations;

    /// <summary>
    /// Creates new instance of <see cref="WavesObservableConfigurableObject"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    protected WavesObservableConfigurableObject(
        IConfiguration configuration,
        ILogger<WavesObservableConfigurableObject> logger)
        : base(logger)
    {
        _configurations = ConfigurableExtensions.InitializeConfiguration(this, configuration);
    }

    /// <inheritdoc />
    public override async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        try
        {
            await LoadConfigurationAsync();
            await RunInitializationAsync();
            IsInitialized = true;
            Logger.LogDebug("Object {@This} initialized", this);
        }
        catch (Exception e)
        {
            IsInitialized = false;
            Logger.LogError("Object initialization error: {Message}", e);
        }
    }

    /// <summary>
    /// Does initialization work.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override Task RunInitializationAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Loads configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task LoadConfigurationAsync()
    {
        foreach (var pair in _configurations)
        {
            Logger.LogDebug("Configuration of {@This}: {PairKey} - {PairValue}", this, pair.Key, pair.Value);
        }

        return this.Configure(_configurations, Logger);
    }
}
