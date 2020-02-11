using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    [Serializable]
    public struct Color : IColor
    {
        public static Color Red = FromArgb(255, 255, 0, 0);
        public static Color Green = FromArgb(255, 0, 255, 0);
        public static Color Blue = FromArgb(255, 0, 0, 255);
        public static Color White = FromArgb(255, 255, 255, 255);
        public static Color Gray = FromArgb(255, 100, 100, 100);
        public static Color Black = FromArgb(255, 0, 0, 0);
        public static Color Empty = FromArgb(0, 0, 0, 0);
        public static Color LightGray = FromHex("#d3d3d3");
        public static Color Transparent = Empty;

        /// <summary>
        ///     Произвольный цвет
        /// </summary>
        /// <returns></returns>
        public static Color Random()
        {
            var random = new Random();

            var a = 255;
            var r = random.NextDouble() * 255;
            var g = random.NextDouble() * 255;
            var b = random.NextDouble() * 255;

            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;

            return new Color(Convert.ToByte(a), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }

        /// <summary>
        ///     Red
        /// </summary>
        public byte R { get; }

        /// <summary>
        ///     Green
        /// </summary>
        public byte G { get; }

        /// <summary>
        ///     Blue
        /// </summary>
        public byte B { get; }

        /// <summary>
        ///     Alpha
        /// </summary>
        public byte A { get; }

        /// <summary>
        ///     Нормированное значение красного
        /// </summary>
        public float ScR => R / 255f;

        /// <summary>
        ///     Нормированное значение зеленого
        /// </summary>
        public float ScG => G / 255f;

        /// <summary>
        ///     Нормированное значение синего
        /// </summary>
        public float ScB => B / 255f;

        /// <summary>
        ///     Нормированное значение альфа-канала
        /// </summary>
        public float ScA => A / 255f;

        /// <summary>
        ///     Создает новый экземпляр цвета
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        ///     Создает новый экземпляр цвета
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        ///     Получение цвета из ARGB параметров
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(a, r, g, b);
        }

        /// <summary>
        ///     Преобразование в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"A:{A},R:{R},G:{G},B:{B}";
        }

        /// <summary>
        ///     Сравнение значений
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        /// <summary>
        ///     Сравнение
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Color color && Equals(color);
        }

        /// <summary>
        ///     Получение хеш-кода
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Преобразование из HEX-строки
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static Color FromHex(string hexColor)
        {
            try
            {
                if (string.IsNullOrEmpty(hexColor)) return default;

                hexColor = hexColor.Trim("#".ToCharArray());

                if (hexColor.Length == 8)
                    return FromArgb(
                        Convert.ToByte(hexColor.Substring(0, 2), 16),
                        Convert.ToByte(hexColor.Substring(2, 2), 16),
                        Convert.ToByte(hexColor.Substring(4, 2), 16),
                        Convert.ToByte(hexColor.Substring(6, 2), 16)
                    );

                return FromArgb(
                    255,
                    Convert.ToByte(hexColor.Substring(0, 2), 16),
                    Convert.ToByte(hexColor.Substring(2, 2), 16),
                    Convert.ToByte(hexColor.Substring(4, 2), 16)
                );
            }
            catch (Exception)
            {
                // ignored
            }

            return default;
        }

        /// <summary>
        ///     Преобразование в HEX-строку
        /// </summary>
        /// <param name="isUseAlphaIsSet"></param>
        /// <param name="isHexPrefix"></param>
        /// <returns></returns>
        public string ToHexString(bool isUseAlphaIsSet = true, bool isHexPrefix = true)
        {
            if (A == 255) return isHexPrefix ? $"#{R:X2}{G:X2}{B:X2}" : $"{R:X2}{G:X2}{B:X2}";

            if (isUseAlphaIsSet) return isHexPrefix ? $"#{A:X2}{R:X2}{G:X2}{B:X2}" : $"{A:X2}{R:X2}{G:X2}{B:X2}";

            return isHexPrefix ? $"#{R:X2}{G:X2}{B:X2}" : $"{R:X2}{G:X2}{B:X2}";
        }

        /// <summary>
        ///     Попытка преобразования из hex-строки
        /// </summary>
        /// <param name="hexColor"></param>
        /// <param name="color"></param>
        /// <param name="isHasAlphaInSource"></param>
        /// <returns></returns>
        public static bool TryParseFromHex(string hexColor, out Color color, out bool isHasAlphaInSource)
        {
            color = Black;
            isHasAlphaInSource = false;

            if (string.IsNullOrWhiteSpace(hexColor))
                return false;

            hexColor = hexColor.Trim('#');
            if (hexColor.Length > 8) hexColor = hexColor.Substring(0, 8);

            try
            {
                switch (hexColor.Length)
                {
                    case 8:
                        color = FromArgb(Convert.ToByte(hexColor.Substring(0, 2), 16),
                            Convert.ToByte(hexColor.Substring(2, 2), 16),
                            Convert.ToByte(hexColor.Substring(4, 2), 16),
                            Convert.ToByte(hexColor.Substring(6, 2), 16));
                        isHasAlphaInSource = true;
                        return true;

                    case 6:
                        color = FromArgb(255,
                            Convert.ToByte(hexColor.Substring(0, 2), 16),
                            Convert.ToByte(hexColor.Substring(2, 2), 16),
                            Convert.ToByte(hexColor.Substring(4, 2), 16));
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}