using System;
using System.Runtime.CompilerServices;
using Waves.Core.Base.Enums;

namespace Waves.Core.Base.Attributes;

/// <summary>
///     Attribute for plugin.
/// </summary>
public class WavesPluginAttribute : WavesObjectAttribute
{
    /// <summary>
    ///     Creates new instance of <see cref="WavesPluginAttribute" />.
    /// </summary>
    /// <param name="pluginType">Plugin type.</param>
    /// <param name="lifetimeType">Plugin lifetime type.</param>
    /// <param name="name">Name of plugin.</param>
    public WavesPluginAttribute(
        Type pluginType,
        WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
        [CallerMemberName] string name = default)
        : base(name)
    {
        Type = pluginType;
        Lifetime = lifetimeType;
    }

    /// <summary>
    ///     Creates new instance of <see cref="WavesPluginAttribute" />.
    /// </summary>
    /// <param name="key">Registration key.</param>
    /// <param name="pluginType">Plugin type.</param>
    /// <param name="lifetimeType">Plugin lifetime type.</param>
    /// <param name="name">Name of plugin.</param>
    public WavesPluginAttribute(
        object key,
        Type pluginType,
        WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
        [CallerMemberName] string name = default)
        : base(name)
    {
        Key = key;
        Type = pluginType;
        Lifetime = lifetimeType;
    }

    /// <summary>
    ///     Creates new instance of <see cref="WavesPluginAttribute" />.
    /// </summary>
    /// <param name="id">Id of plugin.</param>
    /// <param name="key">Registration key.</param>
    /// <param name="pluginType">Plugin type.</param>
    /// <param name="lifetimeType">Plugin lifetime type.</param>
    /// <param name="name">Name of plugin.</param>
    public WavesPluginAttribute(
        Guid id,
        object key,
        Type pluginType,
        WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
        [CallerMemberName] string name = default)
        : base(id, name)
    {
        Key = key;
        Type = pluginType;
        Lifetime = lifetimeType;
    }

    /// <summary>
    ///     Creates new instance of <see cref="WavesPluginAttribute" />.
    /// </summary>
    /// <param name="id">Id of plugin.</param>
    /// <param name="key">Registration key.</param>
    /// <param name="pluginType">Plugin type.</param>
    /// <param name="lifetimeType">Plugin lifetime type.</param>
    /// <param name="name">Name of plugin.</param>
    public WavesPluginAttribute(
        string id,
        object key,
        Type pluginType,
        WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
        [CallerMemberName] string name = default)
        : base(id, name)
    {
        Key = key;
        Type = pluginType;
        Lifetime = lifetimeType;
    }

    /// <summary>
    ///     Gets whether plugin must has single instance when registering in container.
    /// </summary>
    public WavesLifetimeType Lifetime { get; }

    /// <summary>
    ///     Gets key.
    /// </summary>
    public object Key { get; }

    /// <summary>
    ///     Gets plugin type.
    /// </summary>
    public Type Type { get; }
}
