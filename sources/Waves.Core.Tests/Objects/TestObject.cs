using System;

namespace Waves.Core.Tests.Objects;

/// <summary>
/// Test object.
/// </summary>
[Serializable]
public class TestObject
{
    /// <summary>
    /// Gets guid.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
}
