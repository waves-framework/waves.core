using System;
using System.Runtime.CompilerServices;

namespace Waves.Core.Base.Attributes;

/// <summary>
/// Base object attribute.
/// It is used for all objects like messages, plugins, services etc.
/// </summary>
public class WavesObjectAttribute : Attribute
{
    /// <summary>
    /// Creates new instance of <see cref="WavesObjectAttribute"/>.
    /// </summary>
    /// <param name="name">Object name.</param>
    /// <param name="isConfigurable">Whether object is configurable.</param>
    public WavesObjectAttribute(
        [CallerMemberName] string name = default,
        bool isConfigurable = false)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsConfigurable = isConfigurable;
    }

    /// <summary>
    /// Creates new instance of <see cref="WavesObjectAttribute"/>.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="name">Object name.</param>
    /// <param name="isConfigurable">Whether object is configurable.</param>
    public WavesObjectAttribute(
        Guid id,
        [CallerMemberName] string name = default,
        bool isConfigurable = false)
    {
        Id = id;
        Name = name;
        IsConfigurable = isConfigurable;
    }

    /// <summary>
    /// Creates new instance of <see cref="WavesObjectAttribute"/>.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="id">Id.</param>
    /// <param name="isConfigurable">Whether object is configurable.</param>
    public WavesObjectAttribute(
        string id,
        [CallerMemberName] string name = default,
        bool isConfigurable = false)
    {
        Id = Guid.Parse(id);
        Name = name;
        IsConfigurable = isConfigurable;
    }

    /// <summary>
    /// Gets Id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets whether object configurable or not.
    /// </summary>
    public bool IsConfigurable { get; }
}
