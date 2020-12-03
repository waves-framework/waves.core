using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base.EventArgs
{
    /// <summary>
    ///     Event args for data receiving handling.
    /// </summary>
    public class WavesDataReceivedEventArgs
    {
        /// <summary>
        ///     Creates new instance of DataReceivedEventArgs.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="data">Data.</param>
        public WavesDataReceivedEventArgs(IWavesObject sender, object data)
        {
            Sender = sender;
            Data = data;
        }

        /// <summary>
        ///     Gets sender.
        /// </summary>
        public IWavesObject Sender { get; }

        /// <summary>
        ///     Gets data.
        /// </summary>
        public object Data { get; }
    }
}