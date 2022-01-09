namespace Waves.Core.Base.Enums;

/// <summary>
/// Enum for type of plugins.
/// </summary>
public enum WavesPluginType
{
    /// <summary>
    /// Container plugin.
    /// </summary>
    Container,

    /// <summary>
    /// Core plugin type.
    /// Plugins of this type are registered in container and initialized before others.
    /// </summary>
    Core,

    /// <summary>
    /// Standard plugin type.
    /// </summary>
    Standard,
}
