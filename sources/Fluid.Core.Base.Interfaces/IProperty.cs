using System;

namespace Fluid.Core.Base.Interfaces
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