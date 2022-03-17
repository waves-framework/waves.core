using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Waves initialization object abstraction.
/// </summary>
public abstract class WavesInitializableObject :
    WavesObject,
    IWavesInitializableObject
{
    /// <summary>
    /// Creates new instance of <see cref="WavesInitializableObject"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    protected WavesInitializableObject(ILogger<WavesInitializableObject> logger)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public bool IsInitialized { get; internal set; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger<WavesInitializableObject> Logger { get; }

    /// <inheritdoc />
    public virtual async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        try
        {
            await RunInitializationAsync();
            IsInitialized = true;
            Logger.LogDebug($"Object {this} initialized");
        }
        catch (Exception e)
        {
            IsInitialized = false;
            Logger.LogError(e, "Object initialization error");
        }
    }

    /// <summary>
    /// Does initialization work.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task RunInitializationAsync()
    {
        return Task.CompletedTask;
    }
}
