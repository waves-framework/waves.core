using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base.EventArgs
{
    /// <summary>
    ///     Pointer event arguments.
    /// </summary>
    public class WavesPointerEventArgs
    {
        /// <summary>
        ///     Создает новый экземпляр события мыши
        /// </summary>
        /// <param name="button"></param>
        /// <param name="type"></param>
        /// <param name="clicks"></param>
        /// <param name="delta"></param>
        /// <param name="location"></param>
        public WavesPointerEventArgs(WavesMouseButton button, WavesPointerEventType type, int clicks, IWavesPoint delta, IWavesPoint location)
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
        public WavesMouseButton Button { get; }

        /// <summary>
        ///     Тип события мыши
        /// </summary>
        public WavesPointerEventType Type { get; }

        /// <summary>
        ///     Количество кликов мыши
        /// </summary>
        public int Clicks { get; }

        /// <summary>
        ///     Значение, на которое повернулось колесо мыши
        /// </summary>
        public IWavesPoint Delta { get; }

        /// <summary>
        ///     Расположение курсора
        /// </summary>
        public IWavesPoint Location { get; }

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