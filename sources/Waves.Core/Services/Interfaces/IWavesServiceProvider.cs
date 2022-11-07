using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Waves.Core.Services.Interfaces;

/// <summary>
/// Interface of waves service provider.
/// </summary>
public interface IWavesServiceProvider
{
    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public T GetInstance<T>(object key = null);

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<T> GetInstanceAsync<T>(object key = null);

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public object GetInstance(Type type, object key = null);

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<object> GetInstanceAsync(Type type, object key = null);

    /// <summary>
    /// Gets instances by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public IEnumerable<T> GetInstances<T>(object key = null);

    /// <summary>
    /// Gets instances by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<IEnumerable<T>> GetInstancesAsync<T>(object key = null)
        where T : class;
}
