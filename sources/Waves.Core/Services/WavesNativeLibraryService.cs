using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
#if NETCOREAPP3_1 || NET5_0 || NET6_0
using System.Runtime.InteropServices;
#elif NETSTANDARD2_0 || NET462
using System.Reflection;
#endif
using System.Threading.Tasks;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Services
{
    /// <summary>
    ///     Service for loading native libraries.
    /// </summary>
    public class WavesNativeLibraryService :
        IWavesNativeLibraryService
    {
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

        /// <inheritdoc />
        public List<string> Names { get; private set; } = new List<string>();

        /// <inheritdoc />
        public async Task UpdateAsync()
        {
            var paths = new List<string>();

            foreach (var directory in _defaultDirectories)
            {
                if (!Directory.Exists(directory))
                {
                    try
                    {
                        if (directory != null)
                        {
                            Directory.CreateDirectory(directory);
                        }
                    }
                    catch (Exception)
                    {
                        // TODO:
                    }
                }

                paths.Add(directory);
            }

            try
            {
                Names.Clear();

                foreach (var path in paths)
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
                            Names.Add(info.FullName);
                        }
                        catch (Exception)
                        {
                            // TODO:
                        }
                    }
                }

                foreach (var directory in _defaultDirectories)
                {
                    paths.Remove(directory);
                }
            }
            catch (Exception)
            {
                // TODO:
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Native Library Service";
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
