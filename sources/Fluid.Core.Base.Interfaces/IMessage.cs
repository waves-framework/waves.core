using System;
using Fluid.Core.Base.Enums;

namespace Fluid.Core.Base.Interfaces
{
    public interface IMessage
    {
        /// <summary>
        ///     Заголовок
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Дата и время
        /// </summary>
        DateTime DateTime { get; }

        /// <summary>
        ///     Описание
        /// </summary>
        string Text { get; }

        /// <summary>
        ///     Имя отправителя
        /// </summary>
        string Sender { get; }

        /// <summary>
        ///     Тип данных
        /// </summary>
        MessageType Type { get; }
    }
}