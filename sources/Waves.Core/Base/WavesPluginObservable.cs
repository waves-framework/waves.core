using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
///     Abstract base for observable plugins.
/// </summary>
public abstract class WavesPluginObservable :
    WavesObjectObservable,
    IWavesPluginObservable
{
}
