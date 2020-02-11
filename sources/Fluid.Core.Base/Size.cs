namespace Fluid.Core.Base
{
    public struct Size
    {
        /// <summary>
        ///     Создает новый экземпляр размера
        /// </summary>
        /// <param name="length"></param>
        public Size(float length)
        {
            Width = length;
            Height = length;
        }

        /// <summary>
        ///     Создает новый экземпляр размера
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        ///     Ширина
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        ///     Высота
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        ///     Площадь
        /// </summary>
        public float Space => Width * Height;

        /// <summary>
        ///     Соотношение сторон
        /// </summary>
        public float Aspect => Width / Height;

        /// <summary>
        ///     Сравнение 2 объектов
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Size other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        /// <summary>
        ///     Сравнение 2 объектов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Size size && Equals(size);
        }

        /// <summary>
        ///     Получение хеш-кода
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
            }
        }
    }
}