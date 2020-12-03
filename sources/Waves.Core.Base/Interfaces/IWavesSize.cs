namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of Size structures.
    /// </summary>
    public interface IWavesSize
    {
        /// <summary>
        ///     Gets or sets width.
        /// </summary>
        float Width { get; }

        /// <summary>
        ///     Gets or sets height.
        /// </summary>
        float Height { get; }

        /// <summary>
        ///     Get or sets space of size structure.
        /// </summary>
        float Space { get; }

        /// <summary>
        ///     Gets or sets aspect ratio of size structure.
        /// </summary>
        float Aspect { get; }
    }
}