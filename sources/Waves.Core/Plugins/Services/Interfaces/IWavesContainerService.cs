using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Plugins.Services.Interfaces
{
    /// <summary>
    ///     Interface for container.
    /// </summary>
    public interface IWavesContainerService : IWavesService
    {
        /// <summary>
        ///     Gets whether container is built.
        /// </summary>
        bool IsBuilt { get; }

        /// <summary>
        ///     Builds container.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task BuildAsync();

        /// <summary>
        ///     Populates services from Microsoft dependency injection mechanism.
        ///     This must be done before building the container.
        /// </summary>
        /// <param name="collection">Collection.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Populate(
            IServiceCollection collection);

        /// <summary>
        ///     Gets instance by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        Task<T> GetInstanceAsync<T>()
            where T : class;

        /// <summary>
        ///     Gets instances by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Instance.</returns>
        Task<IEnumerable<T>> GetInstancesAsync<T>()
            where T : class;

        /// <summary>
        ///     Gets instances by type.
        /// </summary>
        /// <param name="t">Type.</param>
        /// <returns>Instance.</returns>
        Task<object> GetInstanceAsync(
            Type t);

        /// <summary>
        /// Gets instance by type and key.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <returns>Instance.</returns>
        Task<T> GetInstanceAsync<T>(object key)
            where T : class;

        /// <summary>
        /// Gets instance by type and key.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="key">Key.</param>
        /// <returns>Instance.</returns>
        Task<object> GetInstanceAsync(
            Type type,
            object key);

        /// <summary>
        /// Gets instance by type and key.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="key">Key.</param>
        /// <returns>Instances.</returns>
        Task<IEnumerable<T>> GetInstancesAsync<T>(object key)
            where T : class;

        /// <summary>
        ///     Registers instance by contract type.
        /// </summary>
        /// <typeparam name="T">Contract type.</typeparam>
        /// <param name="instance">Instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RegisterInstanceAsync<T>(T instance)
            where T : class;

        /// <summary>
        ///     Registers single instance by contract type.
        /// </summary>
        /// <typeparam name="T">Contract type.</typeparam>
        /// <param name="instance">Instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RegisterSingleInstanceAsync<T>(T instance)
            where T : class;

        /// <summary>
        ///     Registers instance with current key.
        /// </summary>
        /// <typeparam name="T">Contract type.</typeparam>
        /// <param name="type">Type.</param>
        /// <param name="key">Key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RegisterKeyedTypeAsync<T>(Type type, object key)
            where T : class;

        /// <summary>
        ///     Registers instance with current key.
        /// </summary>
        /// <typeparam name="T">Contract type.</typeparam>
        /// <param name="type">Type.</param>
        /// <param name="key">Key.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RegisterSingleKeyedTypeAsync<T>(Type type, object key)
            where T : class;
    }
}
