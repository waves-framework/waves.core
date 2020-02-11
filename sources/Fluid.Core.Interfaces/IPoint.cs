namespace Fluid.Core.Interfaces
{
    public interface IPoint
    {
        /// <summary>
        ///     Получает или задает координату по оси X.
        /// </summary>
        float X { get; set; }

        /// <summary>
        ///     Получает или задает координату по оси Y.
        /// </summary>
        float Y { get; set; }

        /// <summary>
        /// Получает длину вектора.
        /// </summary>
        float Length { get; }

        /// <summary>
        /// Получает длину вектора в квадрате (для оптимизаций).
        /// </summary>
        float SquaredLength { get; }

        /// <summary>
        /// Получает модуль угла вектора.
        /// </summary>
        float Angle { get; }
    }
}