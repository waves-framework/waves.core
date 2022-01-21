using System;
using Newtonsoft.Json;

namespace Waves.Core.Tests.Objects;

/// <summary>
/// Test object.
/// </summary>
[Serializable]
public class TestObject : IEquatable<TestObject>
{
    /// <summary>
    /// Creates new instance of <see cref="TestObject"/>.
    /// </summary>
    public TestObject()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Creates new instance of <see cref="TestObject"/>.
    /// </summary>
    /// <param name="id">Id.</param>
    [JsonConstructor]
    public TestObject(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets guid.
    /// </summary>
    public Guid Id { get; }

    /// <inheritdoc />
    public bool Equals(TestObject? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == this.GetType() && Equals((TestObject)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
