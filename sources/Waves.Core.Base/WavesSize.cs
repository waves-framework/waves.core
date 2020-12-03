using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Size base structure.
    /// </summary>
    public readonly struct WavesSize : IWavesSize
    {
        /// <summary>
        ///     Creates new instance of size (square).
        /// </summary>
        /// <param name="length">Length.</param>
        public WavesSize(float length)
        {
            Width = length;
            Height = length;
        }

        /// <summary>
        ///     Creates new instance of size (rectangle)
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public WavesSize(float width, float height)
        {
            Width = width;
            Height = height;
        }


        /// <inheritdoc />
        public float Width { get; }

        /// <inheritdoc />
        public float Height { get; }

        /// <inheritdoc />
        public float Space => Width * Height;

        /// <inheritdoc />
        public float Aspect => Width / Height;

        /// <summary>
        ///     Gets whether two size structures are equals.
        /// </summary>
        /// <param name="other">The second structure.</param>
        /// <returns>Return true if equals, false if not.</returns>
        public bool Equals(WavesSize other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is WavesSize size && Equals(size);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
            }
        }
    }
}