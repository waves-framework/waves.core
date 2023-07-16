using System;
using System.Runtime.CompilerServices;

namespace Waves.Core.Base.Attributes;

/// <summary>
/// Attribute for property.
/// </summary>
public class WavesPropertyAttribute : Attribute
{
    /// <summary>
    /// Creates new instance of <see cref="WavesPropertyAttribute"/>.
    /// </summary>
    /// <param name="name">Name of attribute.</param>
    public WavesPropertyAttribute([CallerMemberName] string name = default)
    {
        Name = name;
    }

    /// <summary>
    /// Name of property.
    /// </summary>
    public string Name { get; }
}
