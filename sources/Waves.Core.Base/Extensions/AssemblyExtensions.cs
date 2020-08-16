using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
#if NETCOREAPP3_1

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
        /// <param name="path">Path to directory.</param>
        /// <param name="assemblies">Assemblies list.</param>
        public static void GetAssemblies(this ICollection<System.Reflection.Assembly> assemblies, string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException("Directory not found.");

            foreach (var file in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var hasItem = false;
                    var fileInfo = new FileInfo(file);

                    foreach (var assembly in assemblies)
                    {
                        var name = assembly.GetName().Name;

                        if (name == fileInfo.Name.Replace(fileInfo.Extension, "")) hasItem = true;
                    }

                    if (!hasItem) assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(file));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}