namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of color structures.
    /// </summary>
    public interface IColor
    {
        /// <summary>
        ///     Red.
        /// </summary>
        byte R { get; }

        /// <summary>
        ///     Green.
        /// </summary>
        byte G { get; }

        /// <summary>
        ///     Blue.
        /// </summary>
        byte B { get; }

        /// <summary>
        ///     Alpha.
        /// </summary>
        byte A { get; }

        /// <summary>
        ///     Red.
        /// </summary>
        float ScR { get; }

        /// <summary>
        ///     Green.
        /// </summary>
        float ScG { get; }

        /// <summary>
        ///     Blue.
        /// </summary>
        float ScB { get; }

        /// <summary>
        ///     Alpha.
        /// </summary>
        float ScA { get; }

        /// <summary>
        ///     Converts to HEX string.
        /// </summary>
        /// <param name="isUseAlphaIsSet">Display alpha channel.</param>
        /// <param name="isHexPrefix">Display HEX prefix.</param>
        /// <returns>HEX string.</returns>
        string ToHexString(bool isUseAlphaIsSet = true, bool isHexPrefix = true);
    }
}