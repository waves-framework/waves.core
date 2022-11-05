using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Logging;
using Waves.Core.Extensions;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Services;

/// <summary>
/// Waves service provider.
/// </summary>
public class WavesServiceProvider : IWavesServiceProvider
{
    private readonly IContainer _container;
    private readonly ILogger<WavesServiceProvider> _logger;

    /// <summary>
    /// Creates new instance of <see cref="WavesServiceProvider"/>.
    /// </summary>
    /// <param name="container">Container.</param>
    public WavesServiceProvider(
        IContainer container)
    {
        _container = container;
        _logger = _container.Resolve<ILogger<WavesServiceProvider>>();
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public T GetInstance<T>(object key = null)
    {
        var result = key == null
            ? _container.Resolve<T>()
            : _container.ResolveKeyed<T>(key);

#if DEBUG
        _logger.LogDebug("{Name} resolved from container", result.GetType().GetFriendlyName());
#endif

        return result;
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<T> GetInstanceAsync<T>(object key = null)
    {
        var result = key == null
            ? _container.Resolve<T>()
            : _container.ResolveKeyed<T>(key);

#if DEBUG
        _logger.LogDebug("{Name} resolved from container", result.GetType().GetFriendlyName());
#endif

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public object GetInstance(Type type, object key = null)
    {
        var result = key == null
            ? _container.Resolve(type)
            : _container.ResolveKeyed(key, type);

#if DEBUG
        _logger.LogDebug("{Name} resolved from container", result.GetType().GetFriendlyName());
#endif

        return result;
    }

    /// <summary>
    /// Gets instance by type and key.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<object> GetInstanceAsync(Type type, object key = null)
    {
        var result = key == null
            ? _container.Resolve(type)
            : _container.ResolveKeyed(key, type);

#if DEBUG
        _logger.LogDebug("{Name} resolved from container", result.GetType().GetFriendlyName());
#endif

        return Task.FromResult(result);
    }

    /// <summary>
    /// Gets instances by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public IEnumerable<T> GetInstances<T>(object key = null)
    {
        var results = key == null
            ? _container.Resolve<IEnumerable<T>>()
            : _container.ResolveKeyed<IEnumerable<T>>(key);

        var e = results.ToList();
        foreach (var result in e)
        {
#if DEBUG
            _logger.LogDebug("{Name} resolved from container", result.GetType().GetFriendlyName());
#endif
        }

        return e.AsEnumerable();
    }

    /// <summary>
    /// Gets instances by type and key.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Returns instance.</returns>
    public Task<IEnumerable<T>> GetInstancesAsync<T>(object key = null)
        where T : class
    {
        var results = key == null
            ? _container.Resolve<IEnumerable<T>>()
            : _container.ResolveKeyed<IEnumerable<T>>(key);

        var e = results.ToList();
        foreach (var result in e)
        {
#if DEBUG
            var stackTrace = new StackTrace(1, false);
            var type = stackTrace.GetFrame(1)?.GetMethod()?.DeclaringType;
            _logger.LogDebug("{Type} resolved {Name} from container", type.GetFriendlyName(), result.GetType().GetFriendlyName());
#endif
        }

        return Task.FromResult(e.AsEnumerable());
    }
}
