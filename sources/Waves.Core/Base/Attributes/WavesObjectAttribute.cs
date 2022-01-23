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
    public WavesObjectAttribute(
        string name = default)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    /// <summary>
    /// Creates new instance of <see cref="WavesObjectAttribute"/>.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="name">Object name.</param>
    public WavesObjectAttribute(
        Guid id,
        string name = default)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Creates new instance of <see cref="WavesObjectAttribute"/>.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="id">Id.</param>
    public WavesObjectAttribute(
        string id,
        string name = default)
    {
        Id = Guid.Parse(id);
        Name = name;
    }

    /// <summary>
    /// Gets Id.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets name.
    /// </summary>
    public string Name { get; internal set; }
}
