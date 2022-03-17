using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if NET6_0 || NETCOREAPP3_1
using System.Runtime.Loader;
#endif
using System.Threading.Tasks;

namespace Waves.Core.Extensions;

/// <summary>
///     Assembly loading extensions.
/// </summary>
internal static class AssemblyExtensions
{
    /// <summary>
    ///     Gets assemblies from current directory.
    /// </summary>
    /// <param name="assemblies">Assemblies list.</param>
    /// <param name="path">Path to directory.</param>
    /// <param name="exceptions">Returned exceptions.</param>
    /// <param name="searchOption">Search option.</param>
    /// <returns>Collection of assemblies.</returns>
    internal static ICollection<Assembly> GetAssemblies(
        this ICollection<Assembly> assemblies,
        string path,
        out ICollection<Exception> exceptions,
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException("Directory not found.");
        }

        exceptions = new List<Exception>();

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

        return assemblies;
    }

    /// <summary>
    ///     Gets assemblies from current directory.
    /// </summary>
    /// <param name="assemblies">Assemblies list.</param>
    /// <param name="path">Path to directory.</param>
    /// <param name="exceptions">Returned exceptions.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    internal static Task<ICollection<Assembly>> GetAssembliesAsync(
        this ICollection<Assembly> assemblies,
        string path,
        out ICollection<Exception> exceptions)
    {
        return Task.FromResult(GetAssemblies(assemblies, path, out exceptions));
    }
}
