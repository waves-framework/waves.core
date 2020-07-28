using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Services
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
                    MessageType.Warning));

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
                OnMessageReceived(new Message("Getting service", "Error getting service (" + typeof(T) + ").",
                    "Service manager", e, false));
            }

            return collection;
        }

        /// <summary>
        ///     Loads services.
        /// </summary>
        private void LoadServices()
        {
            OnMessageReceived(new Message("Assembly searching", "Searching for assemblies...", "Service manager",
                MessageType.Information));

            var assemblies = new List<Assembly>();
            Extensions.Assembly.GetAssemblies(assemblies, _currentDirectory);

            OnMessageReceived(new Message("Assembly searching", assemblies.Count + " assemblies were found.",
                "Service manager", MessageType.Information));

            if (assemblies.Count == 0) return;

            try
            {
                var configuration = new ContainerConfiguration().WithAssemblies(assemblies);

                using (var container = configuration.CreateContainer())
                {
                    Services = container.GetExports<IService>();

                    foreach (var service in Services)
                        OnMessageReceived(new Message("Assembly loading",
                            "Service assembly \"" + service.Name + "\" loaded.", "Service manager",
                            MessageType.Information));
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(new Message("Assembly loading", "Error loading assemblies.", "Service manager", e,
                    false));
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