using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for core.
    /// </summary>
    public interface IWavesCore : IWavesObject
    {
        /// <summary>
        /// Gets status of core.
        /// </summary>
        WavesCoreStatus Status { get; }

        /// <summary>
        ///     Gets configuration.
        /// </summary>
        IWavesConfiguration Configuration { get; }

        /// <summary>
        ///     Gets collections of registered services.
        /// </summary>
        ICollection<IWavesService> Services { get; }

        /// <summary>
        ///     Gets service initialization information dictionary.
        ///     Dictionary includes info about base service by default.
        /// </summary>
        Dictionary<string, bool> InitializedServices { get; }

        /// <summary>
        ///     Starts core.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops core working.
        /// </summary>
        void Stop();

        /// <summary>
        ///     Saves configuration.
        /// </summary>
        void SaveConfiguration();

        /// <summary>
        ///     Gets instance by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        T GetInstance<T>() where T : class;

        /// <summary>
        ///     Registers instance.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="instance">Instance.</param>
        void RegisterInstance<T>(T instance) where T : class;

        /// <summary>
        ///     Writes text to log.
        /// </summary>
        /// <param name="text">Text.</param>
        void WriteLog(string text);

        /// <summary>
        ///     Writes message to log.
        /// </summary>
        /// <param name="message">Message.</param>
        void WriteLog(IWavesMessage message);

        /// <summary>
        ///     Writes exception to log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="isFatal">Sets whether exception is fatal.</param>
        void WriteLog(Exception exception, string sender, bool isFatal);
    }
}