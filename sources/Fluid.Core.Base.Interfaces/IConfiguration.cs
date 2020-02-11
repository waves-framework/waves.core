using System;
using System.Collections.Generic;

namespace Fluid.Core.Base.Interfaces
{
    public interface IConfiguration : IObject, ICloneable
    {
        /// <summary>
        ///     Коллекция параметров.
        /// </summary>
        ICollection<IProperty> Properties { get; }

        /// <summary>
        ///     Добавления параметра.
        /// </summary>
        /// <param name="property"></param>
        void AddProperty(IProperty property);

        /// <summary>
        ///     Получение значения параметра.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetPropertyValue(string name);

        /// <summary>
        ///     Установка значения параметра.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetPropertyValue(string name, object value);

        /// <summary>
        ///     Удаление параметра.
        /// </summary>
        /// <param name="name"></param>
        void RemoveProperty(string name);

        /// <summary>
        ///     Содержит ли конфигурации параметр.
        /// </summary>
        /// <param name="name"></param>
        bool Contains(string name);

        /// <summary>
        ///     Получение hash-кода объекта.
        /// </summary>
        /// <returns></returns>
        int GetHashCode();

        /// <summary>
        ///     Событие оправки системного сообщения.
        /// </summary>
        event EventHandler<IMessage> MessageReceived;
    }
}