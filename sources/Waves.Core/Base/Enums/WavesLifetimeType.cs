namespace Waves.Core.Base.Enums;

/// <summary>
/// Enum for lifetime of plugins.
/// </summary>
public enum WavesLifetimeType
{
    /// <summary>
    /// Transient.
    /// </summary>
    Transient,

    /// <summary>
    /// Scoped.
    /// </summary>
    Scoped,

    /// <summary>
    /// Singleton.
    /// </summary>
    Singleton,
}
