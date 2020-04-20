using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Services
{
    /// <summary>
    ///     Service manager.
    /// </summary>
    public class Manager
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        /// <summary>
        ///     Gets or sets collection of services.
        /// </summary>
        [ImportMany]
        public IEnumerable<IService> Services { get; set; }

        /// <summary>
        ///     Event for message receiving handling.
        /// </summary>
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Initializes service manager.
        /// </summary>
        public void Initialize()
        {
            LoadServices();
        }

        /// <summary>
        ///     Loads services.
        /// </summary>
        /// <returns></returns>
        public ICollection<T> GetService<T>()
        {
            var collection = new List<T>();

            if (Services == null)
            {
                OnMessageReceived(new Message("Service Manager", "Services not loaded.", "Service manager",
                    MessageType.Fatal));

                return null;
            }

            try
            {
                foreach (var service in Services)
                    if (service is T currentService)
                        collection.Add(currentService);
            }
            catch (Exception e)
            {
                OnMessageReceived(new Message(e, false));
            }

            return collection;
        }

        /// <summary>
        ///     Loads services.
        /// </summary>
        private void LoadServices()
        {
            var assemblies = new List<Assembly>();

            var files = Directory.GetFiles(_currentDirectory, "*.dll", SearchOption.AllDirectories);

            OnMessageReceived(new Message("Assembly loading", "Trying to load assemblies...", "Service manager",
                MessageType.Information));

            foreach (var file in files)
                try
                {
                    var hasItem = false;
                    var fileInfo = new FileInfo(file);

                    OnMessageReceived(new Message("Assembly loading", "Trying to load assembly " + fileInfo.Name,
                        "Service manager", MessageType.Information));

                    foreach (var assembly in assemblies)
                    {
                        var name = assembly.GetName().Name;
                        if (name == fileInfo.Name.Replace(fileInfo.Extension, ""))
                            hasItem = true;
                    }

                    if (!hasItem) assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(file));
                }
                catch (Exception e)
                {
                    OnMessageReceived(new Message(e, false));
                }

            try
            {
                OnMessageReceived(new Message("Assembly loading", "Trying to load suitable assemblies.",
                    "Service manager", MessageType.Information));

                var configuration = new ContainerConfiguration()
                    .WithAssemblies(assemblies);

                using var container = configuration.CreateContainer();
                Services = container.GetExports<IService>();

                foreach (var service in Services)
                    OnMessageReceived(new Message("Assembly loading",
                        "Service assembly \"" + service.Name + "\" loaded.", "Service manager",
                        MessageType.Information));

                OnMessageReceived(new Message("Assembly loading", "Suitable assemblies loaded.", "Service manager",
                    MessageType.Information));
            }
            catch (Exception e)
            {
                OnMessageReceived(new Message(e, false));
            }
        }

        /// <summary>
        ///     Notifies when message received.
        /// </summary>
        /// <param name="e">Message.</param>
        private void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(null, e);
        }
    }
}