using Fluid.Core.Enums;
using Fluid.Core.Interfaces;

namespace Fluid.Core.EventArgs
{
    public class PointerEventArgs
    {
        /// <summary>
        ///     Создает новый экземпляр события мыши
        /// </summary>
        /// <param name="button"></param>
        /// <param name="type"></param>
        /// <param name="clicks"></param>
        /// <param name="delta"></param>
        /// <param name="location"></param>
        public PointerEventArgs(MouseButton button, PointerEventType type, int clicks, IPoint delta, IPoint location)
        {
            Button = button;
            Type = type;
            Clicks = clicks;
            Delta = delta;
            Location = location;
        }

        /// <summary>
        ///     Кнопка мыши, которая была нажата
        /// </summary>
        public MouseButton Button { get; }

        /// <summary>
        ///     Тип события мыши
        /// </summary>
        public PointerEventType Type { get; }

        /// <summary>
        ///     Количество кликов мыши
        /// </summary>
        public int Clicks { get; }

        /// <summary>
        ///     Значение, на которое повернулось колесо мыши
        /// </summary>
        public IPoint Delta { get; }

        /// <summary>
        ///     Расположение курсора
        /// </summary>
        public IPoint Location { get; }

        /// <summary>
        ///     Координата X расположения курсора
        /// </summary>
        public float X => Location.X;

        /// <summary>
        ///     Координата Y расположения курсора
        /// </summary>
        public float Y => Location.Y;
    }
}