using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    /// <summary>
    /// Service manager.
    /// </summary>
    public static class Manager
    {
        private static readonly string CurrentDirectory = Environment.CurrentDirectory;
        
        /// <summary>
        /// Gets default log path.
        /// </summary>
        private static string DefaultServicesPath => Path.Combine(CurrentDirectory, "services");
         
        /// <summary>
        /// Gets or sets collection of services.
        /// </summary>
        [ImportMany]
        private static IEnumerable<IService> Services { get; set; }

        /// <summary>
        /// Creates new instance of service manager.
        /// </summary>
        static Manager()
        {
            LoadServices();
        }

        /// <summary>
        /// Loads services.
        /// </summary>
        /// <returns></returns>
        public static ICollection<T> GetService<T>()
        {
            var collection = new List<T>();
            
            foreach (var service in Services)
            {
                if (service.GetType() == typeof(T))
                    collection.Add((T)service);
            }

            return collection;
        }

        /// <summary>
        /// Loads services.
        /// </summary>
        private static void LoadServices()
        {
            var assemblies = new List<Assembly>();

            if (!Directory.Exists(DefaultServicesPath)) 
                Directory.CreateDirectory(DefaultServicesPath);
            
            var files = Directory.GetFiles(DefaultServicesPath, "*.dll", SearchOption.AllDirectories);

            foreach (var file in files)
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

            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            using var container = configuration.CreateContainer();
            Services = container.GetExports<IService>();
        }
    }
}