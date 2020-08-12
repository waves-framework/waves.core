﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Waves.Core.Base.Enums;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for core.
    /// </summary>
    public interface ICore
    {
        /// <summary>
        ///     Gets whether Core is running.
        /// </summary>
        public bool IsRunning { get; }

        /// <summary>
        ///     Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Gets collections of registered services.
        /// </summary>
        public ICollection<IService> Services { get; }

        /// <summary>
        ///     Gets service initialization information dictionary.
        ///     Dictionary includes info about base service by default.
        /// </summary>
        public Dictionary<string, bool> InitializedServices { get; }

        /// <summary>
        ///     Event for message receiving handling.
        /// </summary>
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Starts core.
        /// </summary>
        public void Start();

        /// <summary>
        ///     Stops core working.
        /// </summary>
        public void Stop();

        /// <summary>
        ///     Saves configuration.
        /// </summary>
        public void SaveConfiguration();

        /// <summary>
        ///     Gets service by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Service.</returns>
        public T GetService<T>();

        /// <summary>
        ///     Registers service.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="instance">Instance.</param>
        public void RegisterService<T>(T instance);

        /// <summary>
        ///     Writes text to log.
        /// </summary>
        /// <param name="text">Text.</param>
        public void WriteLog(string text);

        /// <summary>
        ///     Writes message to log.
        /// </summary>
        /// <param name="message">Message.</param>
        public void WriteLogMessage(IMessage message);

        /// <summary>
        ///     Writes exception to log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="isFatal">Sets whether exception is fatal.</param>
        public void WriteLogException(Exception exception, string sender, bool isFatal);

        /// <summary>
        ///     Adds log separator.
        /// </summary>
        public void WriteLogSeparator();
    }
}