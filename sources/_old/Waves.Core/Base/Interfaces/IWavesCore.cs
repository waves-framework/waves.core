using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waves.Core.Base.Enums;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for core.
    /// </summary>
    public interface IWavesCore : IWavesObject
    {
        /// <summary>
        /// Message received event.
        /// </summary>
        event EventHandler<IWavesMessageObject> MessageReceived;

        /// <summary>
        ///     Gets status of core.
        /// </summary>
        CoreStatus Status { get; }

        /// <summary>
        /// Starts core.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StartAsync();

        /// <summary>
        ///     Stops core working.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StopAsync();

        /// <summary>
        /// Builds container.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task BuildContainerAsync();

        /// <summary>
        ///     Gets instance by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        Task<T> GetInstanceAsync<T>()
            where T : class;

        /// <summary>
        /// Gets instance by type and key.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <returns>Returns instance.</returns>
        Task<T> GetInstanceAsync<T>(object key)
            where T : class;

        /// <summary>
        ///     Gets instances by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        Task<IEnumerable<T>> GetInstancesAsync<T>()
            where T : class;

        /// <summary>
        /// Gets instances by type and key.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <returns>Returns instance.</returns>
        Task<IEnumerable<T>> GetInstancesAsync<T>(object key)
            where T : class;

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
