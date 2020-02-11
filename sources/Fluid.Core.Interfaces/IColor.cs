namespace Fluid.Core.Interfaces
{
    public interface IColor
    {
        /// <summary>
        ///     Red
        /// </summary>
        byte R { get; }

        /// <summary>
        ///     Green
        /// </summary>
        byte G { get; }

        /// <summary>
        ///     Blue
        /// </summary>
        byte B { get; }

        /// <summary>
        ///     Alpha
        /// </summary>
        byte A { get; }

        /// <summary>
        ///     Нормированное значение красного
        /// </summary>
        float ScR { get; }

        /// <summary>
        ///     Нормированное значение зеленого
        /// </summary>
        float ScG { get; }

        /// <summary>
        ///     Нормированное значение синего
        /// </summary>
        float ScB { get; }

        /// <summary>
        ///     Нормированное значение альфа-канала
        /// </summary>
        float ScA { get; }

        /// <summary>
        /// Перевод в HEX-строку.
        /// </summary>
        /// <param name="isUseAlphaIsSet"></param>
        /// <param name="isHexPrefix"></param>
        /// <returns></returns>
        string ToHexString(bool isUseAlphaIsSet = true, bool isHexPrefix = true);
    }
}