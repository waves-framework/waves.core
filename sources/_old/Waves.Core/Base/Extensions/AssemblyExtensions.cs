using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
#if NETCOREAPP3_1 || NET5_0 || NET6_0
#endif

namespace Waves.Core.Base.Extensions
{
    /// <summary>
    ///     Assembly loading extensions.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Gets assemblies from current directory.
        /// </summary>
        /// <param name="assemblies">Assemblies list.</param>
        /// <param name="path">Path to directory.</param>
        /// <param name="searchOption">Search option.</param>
        public static ICollection<Assembly> GetAssemblies(
            this ICollection<Assembly> assemblies,
            string path,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Directory not found.");
            }

            var exceptions = new List<Exception>();

            foreach (var file in Directory.GetFiles(
                path,
                "*.dll",
                searchOption))
            {
                try
                {
                    var hasItem = false;
                    var fileInfo = new FileInfo(file);

                    foreach (var assembly in assemblies)
                    {
                        var name = assembly.GetName().Name;

                        if (name ==
                            fileInfo.Name.Replace(
                                fileInfo.Extension,
                                string.Empty))
                        {
                            hasItem = true;
                        }
                    }

                    if (!hasItem)
                    {
#if NETCOREAPP3_1 || NET5_0 || NET6_0
                        assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(file));
#elif NETSTANDARD2_0 || NET462
                        assemblies.Add(Assembly.LoadFile(file));
#endif
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }

            return assemblies;
        }

        /// <summary>
        ///     Gets assemblies from current directory.
        /// </summary>
        /// <param name="assemblies">Assemblies list.</param>
        /// <param name="path">Path to directory.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task<ICollection<Assembly>> GetAssembliesAsync(
            this ICollection<Assembly> assemblies,
            string path)
        {
            return Task.FromResult(GetAssemblies(assemblies, path));
        }
    }
}
