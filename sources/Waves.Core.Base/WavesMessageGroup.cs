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
    public class WavesMessageGroup : IWavesMessageGroup
    {
        /// <summary>
        ///     Creates nes instance of <see cref="WavesMessageGroup" />
        /// </summary>
        /// <param name="title"></param>
        /// <param name="sender">Sender.</param>
        /// <param name="dataTime"></param>
        /// <param name="type">Type.</param>
        public WavesMessageGroup(string title, string sender, DateTime dataTime, WavesMessageType type)
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
        public WavesMessageType Type { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public string Sender { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesMessage> Messages { get; } = new ObservableCollection<IWavesMessage>();
    }
}