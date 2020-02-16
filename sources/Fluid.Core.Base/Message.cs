using System;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    public struct Message : IMessage
    {
        /// <summary>
        ///     Создает новое системное сообщение
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="text">Текст сообщения</param>
        /// <param name="sender">Имя отправителя</param>
        /// <param name="type">Тип</param>
        public Message(string title, string text, string sender, MessageType type)
        {
            Title = title;
            Text = text;
            Type = type;
            Sender = sender;
            DateTime = DateTime.Now;
        }

        /// <summary>
        ///     Заголовок
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Дата и время
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        ///     Описание
        /// </summary>
        public string Text { get; }

        /// <summary>
        ///     Имя отправителя
        /// </summary>
        public string Sender { get; }

        /// <summary>
        ///     Тип данных
        /// </summary>
        public MessageType Type { get; }
    }
}