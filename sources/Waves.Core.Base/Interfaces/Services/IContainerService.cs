﻿using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    /// Interface for container
    /// </summary>
    public interface IContainerService : IWavesService
    {
        /// <summary>
        /// Gets whether container is built.
        /// </summary>
        public bool IsBuilt { get; }

        /// <summary>
        /// Builds container.
        /// </summary>
        public void Build();

        /// <summary>
        ///     Gets instance by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        public T GetInstance<T>() where T : class;

        /// <summary>
        ///     Registers instance.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="instance">Instance.</param>
        public void RegisterInstance<T>(T instance) where T : class;
    }
}