namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of point structures.
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        ///     Gets or sets X coordinate.
        /// </summary>
        float X { get; set; }

        /// <summary>
        ///     Get or sets Y coordinate.
        /// </summary>
        float Y { get; set; }

        /// <summary>
        ///     Gets length of current vector.
        /// </summary>
        float Length { get; }

        /// <summary>
        ///     Get square length of current vector.
        /// </summary>
        float SquaredLength { get; }

        /// <summary>
        ///     Gets abs of vector angle.
        /// </summary>
        float Angle { get; }
    }
}