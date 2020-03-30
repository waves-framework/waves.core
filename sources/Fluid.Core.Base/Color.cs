using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Base color structure.
    /// </summary>
    [Serializable]
    public struct Color : IColor
    {
        /// <summary>
        ///     Red color.
        /// </summary>
        public static readonly Color Red = FromArgb(255, 255, 0, 0);

        /// <summary>
        ///     Green color.
        /// </summary>
        public static readonly Color Green = FromArgb(255, 0, 255, 0);

        /// <summary>
        ///     Blue color.
        /// </summary>
        public static readonly Color Blue = FromArgb(255, 0, 0, 255);

        /// <summary>
        ///     White color.
        /// </summary>
        public static readonly Color White = FromArgb(255, 255, 255, 255);

        /// <summary>
        ///     Gray color.
        /// </summary>
        public static readonly Color Gray = FromArgb(255, 100, 100, 100);

        /// <summary>
        ///     Black color.
        /// </summary>
        public static readonly Color Black = FromArgb(255, 0, 0, 0);

        /// <summary>
        ///     Transparent color.
        /// </summary>
        public static readonly Color Transparent = FromArgb(0, 0, 0, 0);

        /// <summary>
        ///     Light gray color.
        /// </summary>
        public static readonly Color LightGray = FromHex("#d3d3d3");

        /// <summary>
        ///     Creates random color.
        /// </summary>
        /// <returns>Random color.</returns>
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


        /// <inheritdoc />
        public byte R { get; }

        /// <inheritdoc />
        public byte G { get; }

        /// <inheritdoc />
        public byte B { get; }

        /// <inheritdoc />
        public byte A { get; }

        /// <inheritdoc />
        public float ScR => R / 255f;

        /// <inheritdoc />
        public float ScG => G / 255f;

        /// <inheritdoc />
        public float ScB => B / 255f;

        /// <inheritdoc />
        public float ScA => A / 255f;

        /// <summary>
        ///     Creates new instance of color structure.
        /// </summary>
        /// <param name="a">Alpha value.</param>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        ///     Creates new instance of color structure.
        /// </summary>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        public Color(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        ///     Gets color from ARGB values.
        /// </summary>
        /// <param name="a">Alpha value.</param>
        /// <param name="r">Red value.</param>
        /// <param name="g">Green value.</param>
        /// <param name="b">Blue value.</param>
        /// <returns>Color.</returns>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(a, r, g, b);
        }

        /// <summary>
        ///     Converts color to string.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString()
        {
            return $"A:{A},R:{R},G:{G},B:{B}";
        }

        /// <summary>
        ///     Compares two colors.
        /// </summary>
        /// <param name="other">Other color.</param>
        /// <returns>True if equals, false if not.</returns>
        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Color color && Equals(color);
        }


        /// <inheritdoc />
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

        /// <inheritdoc />
        public string ToHexString(bool isUseAlphaIsSet = true, bool isHexPrefix = true)
        {
            if (A == 255) return isHexPrefix ? $"#{R:X2}{G:X2}{B:X2}" : $"{R:X2}{G:X2}{B:X2}";

            if (isUseAlphaIsSet) return isHexPrefix ? $"#{A:X2}{R:X2}{G:X2}{B:X2}" : $"{A:X2}{R:X2}{G:X2}{B:X2}";

            return isHexPrefix ? $"#{R:X2}{G:X2}{B:X2}" : $"{R:X2}{G:X2}{B:X2}";
        }

        /// <summary>
        ///     Converts HEX string to Color.
        /// </summary>
        /// <param name="hexColor">HEX string.</param>
        /// <returns>Color.</returns>
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
        ///     Tries parsing HEX string to color.
        /// </summary>
        /// <param name="hexColor">HEX string.</param>
        /// <param name="color">Output color.</param>
        /// <param name="isHasAlphaInSource">Whether color has alpha channel in HEX.</param>
        /// <returns>True if parsed, false if not.</returns>
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