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
        /// <param name="dataTime"></param>
        public MessageGroup(string title, DateTime dataTime)
        {
            Title = title;
            DateTime = dataTime;
        }

        /// <inheritdoc />
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public DateTime DateTime { get; }

        /// <inheritdoc />
        public ICollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();
    }
}