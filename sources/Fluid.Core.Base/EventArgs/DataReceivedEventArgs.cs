using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base.EventArgs
{
    /// <summary>
    ///     Event args for data receiving handling.
    /// </summary>
    public class DataReceivedEventArgs
    {
        /// <summary>
        ///     Creates new instance of DataReceivedEventArgs.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="data">Data.</param>
        public DataReceivedEventArgs(IObject sender, object data)
        {
            Sender = sender;
            Data = data;
        }

        /// <summary>
        ///     Gets sender.
        /// </summary>
        public IObject Sender { get; }

        /// <summary>
        ///     Gets data.
        /// </summary>
        public object Data { get; }
    }
}