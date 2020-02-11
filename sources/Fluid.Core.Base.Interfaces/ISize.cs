namespace Fluid.Core.Base.Interfaces
{
    public interface ISize
    {
        /// <summary>
        ///     Получает или задает ширину.
        /// </summary>
        float Width { get; set; }

        /// <summary>
        ///     Получает или задает высоту.
        /// </summary>
        float Height { get; set; }

        /// <summary>
        ///     Получает или задает площадь.
        /// </summary>
        float Space { get; }

        /// <summary>
        ///     Получает или задает соотношение сторон.
        /// </summary>
        float Aspect { get; }
}
}