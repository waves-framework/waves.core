using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Plugins.Services.Interfaces;

namespace Waves.Core.Plugins.Services
{
    /// <summary>
    /// Service for load types from assemblies.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    internal class WavesTypeLoaderService<T> : WavesService, IWavesTypeLoaderService
    {
        private readonly IWavesCore _core;

        private readonly string _basePluginsDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Creates new instance of <see cref="WavesTypeLoaderService{T}"/>.
        /// </summary>
        /// <param name="core">Instance of core.</param>
        public WavesTypeLoaderService(IWavesCore core)
        {
            _core = core;
        }

        /// <inheritdoc />
        public ICollection<Type> Types { get; } = new List<Type>();

        /// <inheritdoc />
        public override async Task InitializeAsync()
        {
            await UpdateAsync();
        }

#pragma warning disable SA1124 // Do not use regions
        #region Update

        /// <summary>
        /// Updates types.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task UpdateAsync()
        {
            Types.Clear();

            var assemblies = new List<Assembly>();

            try
            {
                await Base.Extensions.AssemblyExtensions.GetAssembliesAsync(
                    assemblies,
                    _basePluginsDirectory);
            }
            catch (Exception e)
            {
                await _core.WriteLogAsync(e, this);
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        var attributes = type.GetCustomAttributes();

                        foreach (var attribute in attributes)
                        {
                            if (attribute is not T)
                            {
                                continue;
                            }

                            Types.Add(type);
                        }
                    }
                }
                catch (Exception e)
                {
                    await _core.WriteLogAsync(e, this);
                }
            }
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return "Type Loader Service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
        }
    }
}
