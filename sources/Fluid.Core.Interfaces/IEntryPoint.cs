using System;

namespace Fluid.Core.Interfaces
{
    public interface IEntryPoint : IObject, IDisposable
    {
        /// <summary>
        ///     Является ли точка свойством.
        /// </summary>
        bool IsProperty { get; set; }

        /// <summary>
        ///     Значение точки.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///     Родительский модуль точки.
        /// </summary>
        IModule Parent { get; }

        /// <summary>
        ///     Получение hash-кода объекта.
        /// </summary>
        /// <returns></returns>
        int GetHashCode();

        /// <summary>
        ///     Событие отправки системного сообщения.
        /// </summary>
        event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Событие приема данных на точку.
        /// </summary>
        event EventHandler<object> DataReceived;
    }
}