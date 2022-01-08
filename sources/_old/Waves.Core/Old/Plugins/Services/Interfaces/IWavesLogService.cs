using System;
using System.Threading.Tasks;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Old.Plugins.Services.Interfaces
{
    /// <summary>
    ///     Interface for logging services.
    /// </summary>
    public interface IWavesLogService : IWavesService
    {
        /// <summary>
        ///     Writes text to log.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WriteLogAsync(
            string text);

        /// <summary>
        ///     Writes <see cref="IWavesMessageObject" /> to log.
        /// </summary>
        /// <param name="message">Instance of <see cref="IWavesMessageObject" />.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WriteLogAsync(
            IWavesMessageObject message);

        /// <summary>
        ///     Writes message to log.
        /// </summary>
        /// <param name="title">Message title.</param>
        /// <param name="text">Message text.</param>
        /// <param name="sender">Message sender.</param>
        /// <param name="type">Message type.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WriteLogAsync(
            string title,
            string text,
            IWavesObject sender,
            WavesMessageType type);

        /// <summary>
        ///     Writes <see cref="Exception" /> to log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="sender">Message sender.</param>
        /// <param name="isFatal">Sets whether error is fatal.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WriteLogAsync(
            Exception exception,
            IWavesObject sender,
            bool isFatal = false);
    }
}
