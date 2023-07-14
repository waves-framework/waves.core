using System;
using System.Runtime.CompilerServices;
using Waves.Core.Base.Enums;
using Waves.Core.Extensions;

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
        WavesLifetime lifetimeType = WavesLifetime.Transient,
        [CallerMemberName] string name = default)
        : base(name)
    {
        Type = pluginType;
        Name = pluginType.ToString();
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
        WavesLifetime lifetimeType = WavesLifetime.Transient,
        [CallerMemberName] string name = default)
        : base(name)
    {
        Key = key;
        Type = pluginType;
        Name = pluginType.ToString();
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
        WavesLifetime lifetimeType = WavesLifetime.Transient,
        [CallerMemberName] string name = default)
        : base(id, name)
    {
        Key = key;
        Type = pluginType;
        Name = pluginType.ToString();
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
        WavesLifetime lifetimeType = WavesLifetime.Transient,
        [CallerMemberName] string name = default)
        : base(id, name)
    {
        Key = key;
        Type = pluginType;
        Name = pluginType.ToString();
        Lifetime = lifetimeType;
    }

    /// <summary>
    ///     Gets whether plugin must has single instance when registering in container.
    /// </summary>
    public WavesLifetime Lifetime { get; }

    /// <summary>
    ///     Gets key.
    /// </summary>
    public object Key { get; }

    /// <summary>
    ///     Gets plugin type.
    /// </summary>
    public Type Type { get; }
}
