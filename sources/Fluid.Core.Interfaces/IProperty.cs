using System;
using System.ComponentModel;

namespace Fluid.Core.Interfaces
{
    public interface IProperty : IObject, ICloneable
    {
        /// <summary>
        ///     Только для чтения.
        /// </summary>
        bool IsReadOnly { get; set; }

        /// <summary>
        ///     Значение свойства.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///     Получение hash-code объекта.
        /// </summary>
        /// <returns></returns>
        int GetHashCode();
    }
}