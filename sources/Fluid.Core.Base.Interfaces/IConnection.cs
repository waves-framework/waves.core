using System;
using System.ComponentModel;

namespace Fluid.Core.Base.Interfaces
{
    public interface IConnection : IObject, INotifyPropertyChanged, IDisposable, ICloneable
    {
        /// <summary>
        ///     Точка входа в соединение.
        /// </summary>
        IEntryPoint Input { get; set; }

        /// <summary>
        ///     Точка выхода из соединения.
        /// </summary>
        IEntryPoint Output { get; set; }

        /// <summary>
        ///     Получение Hash-кода объекта.
        /// </summary>
        /// <returns></returns>
        int GetHashCode();
    }
}