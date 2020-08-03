using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Message group.
    /// </summary>
    public class MessageGroup : IMessageGroup
    {
        /// <summary>
        ///     Creates nes instance of <see cref="MessageGroup" />
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
        public Guid Id { get; internal set; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
        public string Title { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public DateTime DateTime { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public MessageType Type { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public string Sender { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public ICollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();
    }
}