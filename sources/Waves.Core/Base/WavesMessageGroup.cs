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
    public class WavesMessageGroup : 
        WavesMessageObject,
        IWavesMessageGroup
    {
        /// <summary>
        ///     Creates nes instance of <see cref="WavesMessageGroup" />.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="dataTime">Date time.</param>
        /// <param name="type">Type.</param>
        public WavesMessageGroup(
            string title,
            IWavesObject sender,
            DateTime dataTime,
            WavesMessageType type)
        {
            Title = title;
            DateTime = dataTime;
            Sender = sender;
            Type = type;
        }

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesMessageObject> Messages { get; protected set; } = new ObservableCollection<IWavesMessageObject>();
    }
}
