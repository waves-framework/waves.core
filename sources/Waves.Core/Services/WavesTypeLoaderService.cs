using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Extensions;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Services
{
    /// <summary>
    /// Service for load types from assemblies.
    /// </summary>
    /// <typeparam name="T">Attribute type.</typeparam>
    public sealed class WavesTypeLoaderService<T> : WavesPlugin, IWavesTypeLoaderService<T>
    {
        private readonly string _basePluginsDirectory = System.IO.Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);

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
            await assemblies.GetAssembliesAsync(_basePluginsDirectory);

            foreach (var assembly in assemblies)
            {
                try
                {
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
                        }
                    }
                }
                catch (Exception e)
                {
                    // TODO:
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
