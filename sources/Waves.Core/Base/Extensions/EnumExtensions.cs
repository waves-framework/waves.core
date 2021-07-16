using System;
using System.ComponentModel;

namespace Waves.Core.Base.Extensions
{
    /// <summary>
    /// Enum extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets description for current enum value.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Enumeration value.</param>
        /// <returns>Enum value description.</returns>
        public static string ToDescription<T>(this T value)
            where T : struct
        {
            var type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(value));
            }

            // Tries to find a DescriptionAttribute for a potential friendly name
            //  for the enum.
            var memberInfo = type.GetMember(value.ToString() ?? string.Empty);
            if (memberInfo.Length <= 0)
            {
                return value.ToString();
            }

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            // If we have no description attribute, just return the ToString of the enum
            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : value.ToString();
        }
    }
}
