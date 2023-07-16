using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Abstraction for observable initializable object.
/// </summary>
public abstract class WavesObservableInitializableObject :
    WavesObservableObject,
    IWavesObservableInitializableObject
{
    /// <summary>
    /// Creates new instance of <see cref="WavesObservableInitializableObject"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    protected WavesObservableInitializableObject(ILogger<WavesObservableInitializableObject> logger)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public bool IsInitialized { get; protected set; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger<WavesObservableInitializableObject> Logger { get; }

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
            Logger.LogDebug("Object {@This} initialized", this);
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
