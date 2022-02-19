using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Extensions;
using Waves.Core.Services.Interfaces;
using ILogger = NLog.ILogger;

namespace Waves.Core.Services
{
    /// <summary>
    /// Service for load types from assemblies.
    /// </summary>
    /// <typeparam name="T">Attribute type.</typeparam>
    public sealed class WavesTypeLoaderService<T> : IWavesTypeLoaderService<T>
    {
        private readonly ILogger<WavesTypeLoaderService<T>> _logger;

        private readonly string _basePluginsDirectory = System.IO.Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Creates new instance of <see cref="WavesTypeLoaderService{T}"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public WavesTypeLoaderService(ILogger<WavesTypeLoaderService<T>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets types.
        /// </summary>
        public Dictionary<Type, T> Types { get; private set; }

        /// <summary>
        /// Updates types.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateTypesAsync()
        {
            Types ??= new Dictionary<Type, T>();
            Types.Clear();

            var assemblies = new List<Assembly>();
            await assemblies.GetAssembliesAsync(_basePluginsDirectory, out var exceptions);

            if (exceptions != null)
            {
                foreach (var e in exceptions)
                {
                    _logger.LogWarning(e, $"Error occured while loading assembly: {e}");
                }
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    var count = 0;
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        var attributes = type.GetCustomAttributes();
                        foreach (var attribute in attributes)
                        {
                            if (attribute is not T typeAttribute)
                            {
                                continue;
                            }

                            Types.Add(type, typeAttribute);
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        _logger.LogDebug($"Assembly {assembly.FullName} loaded with {count} types");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"Error occured while loading assembly {assembly.FullName}");
                }
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Type Loader Service";
        }
    }
}
