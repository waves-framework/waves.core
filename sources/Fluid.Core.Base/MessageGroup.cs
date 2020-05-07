using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    /// Message group.
    /// </summary>
    public class MessageGroup : IMessageGroup
    {
        /// <summary>
        /// Creates nes instance of <see cref="MessageGroup"/>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="sender">Sender.</param>
        /// <param name="dataTime"></param>
        /// <param name="type">Type.</param>
        public MessageGroup(string title, string sender, DateTime dataTime, MessageType type)
        {
            Title = title;
            DateTime = dataTime;
            Sender = sender;
            Type = type;
        }

        /// <inheritdoc />
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public DateTime DateTime { get; }

        /// <inheritdoc />
        public MessageType Type { get; }

        /// <inheritdoc />
        public string Sender { get; }

        /// <inheritdoc />
        public ICollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();
    }
}