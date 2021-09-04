using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Plugins.Services.Interfaces;

namespace Waves.Core.Plugins.Services
{
    /// <summary>
    ///     Service for loading native libraries.
    /// </summary>
    [WavesService(
        "93364C78-F884-4F47-8B0A-D60976E02DA8",
        typeof(IWavesNativeLibraryService))]
    public class WavesNativeLibraryService :
        WavesConfigurableService,
        IWavesNativeLibraryService
    {
        private readonly IWavesCore _core;

        private readonly IEnumerable<string> _defaultDirectories = new List<string>
        {
            Path.Combine(
                Environment.CurrentDirectory,
                "native"),
            Path.Combine(
                Environment.CurrentDirectory,
                "runtimes"),
        };

        private readonly IEnumerable<string> _defaultFileExtensions = new List<string>
        {
            ".dll",
            ".so",
            ".dylib",
        };

        /// <summary>
        /// Creates new instance of <see cref="WavesNativeLibraryService"/>.
        /// </summary>
        /// <param name="core">Instance of core.</param>
        /// <param name="configurationService">Instance of configuration service.</param>
        public WavesNativeLibraryService(IWavesCore core, IWavesConfigurationService configurationService)
            : base(configurationService)
        {
            _core = core;
        }

        /// <inheritdoc />
        [Reactive]
        [WavesProperty]
        public List<string> Paths { get; private set; } = new List<string>();

        /// <inheritdoc />
        [Reactive]
        public List<string> Names { get; private set; } = new List<string>();

        /// <inheritdoc />
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            if (IsInitialized)
            {
                return;
            }

            await UpdateAsync();
        }

        /// <inheritdoc />
        public Task AddPathAsync(string path)
        {
            if (!Paths.Contains(path))
            {
                Paths.Add(path);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RemovePathAsync(string path)
        {
            if (Paths.Contains(path))
            {
                Paths.Remove(path);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task UpdateAsync()
        {
            foreach (var directory in _defaultDirectories)
            {
                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch (Exception e)
                    {
                        await _core.WriteLogAsync(
                            e,
                            this);
                    }
                }

                Paths.Add(directory);
            }

            try
            {
                Names.Clear();

                foreach (var path in Paths)
                {
                    var files = new List<string>();

                    foreach (var extension in _defaultFileExtensions)
                    {
                        files.AddRange(await SearchFilesAsync(
                            path,
                            extension));
                    }

                    foreach (var file in files)
                    {
                        if (!File.Exists(file))
                        {
                            continue;
                        }

                        var info = new FileInfo(file);

                        try
                        {
#if NETCOREAPP3_1 || NET5_0 || NET6_0
                            var fileName = info.FullName;

                            var result = NativeLibrary.Load(fileName);

                            if (result == IntPtr.Zero)
                            {
                                var lastError = Marshal.GetLastWin32Error();
                                var error = new Win32Exception(lastError);
                                throw error;
                            }
#elif NETSTANDARD2_0 || NET462
                            Assembly.LoadFile(file);
#endif

                            await _core.WriteLogAsync(new WavesTextMessage(
                                $"Native library {info.Name} has been loaded.",
                                "Loading native library",
                                this));

                            Names.Add(info.FullName);
                        }
                        catch (Exception)
                        {
                            await _core.WriteLogAsync(new WavesTextMessage(
                                $"Library {info.FullName} can't be loaded on current system.",
                                "Native library loading error",
                                this,
                                WavesMessageType.Error));
                        }
                    }
                }

                foreach (var directory in _defaultDirectories)
                {
                    Paths.Remove(directory);
                }
            }
            catch (Exception e)
            {
                await _core.WriteLogAsync(new WavesExceptionMessage(
                    this,
                    e,
                    false,
                    "Loading native libraries",
                    "Native libraries have not been loaded."));
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Native Library Service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: your code for release managed resources.
            }

            // TODO: your code for release unmanaged resources.
        }

        /// <summary>
        ///     Searches files in current path within all directories by current exntesion.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Returns files collection.</returns>
        private IEnumerable<string> SearchFiles(
            string path,
            string extension)
        {
            return Directory.EnumerateFiles(
                    path,
                    "*.*",
                    SearchOption.AllDirectories)
                .Where(x => !x.EndsWith(extension));
        }

        /// <summary>
        ///     Searches files in current path within all directories by current extension.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Returns files collection.</returns>
        private Task<IEnumerable<string>> SearchFilesAsync(
            string path,
            string extension)
        {
            return Task.FromResult(Directory.EnumerateFiles(
                    path,
                    "*.*",
                    SearchOption.AllDirectories)
                .Where(x => !x.EndsWith(extension)));
        }
    }
}
