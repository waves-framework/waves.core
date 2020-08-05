using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using AssemblyExtensions = Waves.Core.Base.Extensions.AssemblyExtensions;

namespace Waves.Core.Base
{
    /// <summary>
    /// Abstract MEF objects loader service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MefLoaderService<T>: Service, IMefLoaderService<T> where T: IObject
    {
        /// <inheritdoc />
        public event EventHandler ObjectsUpdated;

        /// <inheritdoc />
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract override string Name { get; set; }

        /// <inheritdoc />
        public List<string> Paths { get; private set; } = new List<string>();

        /// <inheritdoc />
        public IEnumerable<T> Objects { get; private set; }

        /// <summary>
        /// Gets instance of Core.
        /// </summary>
        public ICore Core { get; private set; }

        /// <inheritdoc />
        public override void Dispose()
        {
            UnsubscribeEvents();
        }

        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            if (IsInitialized) return;

            Core = core;

            try
            {
                Update();

                IsInitialized = true;

                OnMessageReceived(this,
                    new Message(
                        "Initialization",
                        "Service has been initialized.",
                        Name,
                        MessageType.Information));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Service initialization", "Error service initialization.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            try
            {
                Paths.AddRange(LoadConfigurationValue(configuration,  Name + "-Paths", new List<string>()));

                OnMessageReceived(this, new Message("Loading configuration", "Configuration loads successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Loading configuration", "Error loading configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            try
            {
                if (Paths.Count > 0)
                {
                    configuration.SetPropertyValue(Name + "-Paths", Paths);

                    OnMessageReceived(this, new Message("Saving configuration", "Configuration saves successfully.",
                        Name,
                        MessageType.Success));
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Saving configuration", "Error saving configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void AddPath(string path)
        {
            try
            {
                if (!Paths.Contains(path)) Paths?.Add(path);

                OnMessageReceived(this, new Message("Adding path", "Path added successfully.",
                    Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Adding path", "Path has not been added.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void RemovePath(string path)
        {
            try
            {
                if (Paths.Contains(path)) Paths?.Remove(path);

                OnMessageReceived(this, new Message("Removing path",
                    "Path removed successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Removing path", "Path has not been removed.", Name, e,
                        false));
            }
        }

        /// <inheritdoc />
        public virtual void Update()
        {
            try
            {
                UnsubscribeEvents();

                var assemblies = new List<Assembly>();

                foreach (var path in Paths)
                    AssemblyExtensions.GetAssemblies(assemblies, path);

                var configuration = new ContainerConfiguration().WithAssemblies(assemblies);

                using (var container = configuration.CreateContainer())
                {
                    Objects = container.GetExports<T>();

                    SubscribeEvents();

                    OnObjectsUpdated();

                    if (Objects != null)
                    {
                        var objects = Objects as T[] ?? Objects.ToArray();

                        if (!objects.Any())
                            OnMessageReceived(this,
                                new Message("Loading objects", "Objects not found.", Name, MessageType.Warning));
                        else
                            OnMessageReceived(this, new Message("Loading objects",
                                "Objects loads successfully (" + objects.Count() + " objects).", Name,
                                MessageType.Success));
                    }
                    else
                    {
                        OnMessageReceived(this,
                            new Message("Loading objects", "Objects were not loaded.", Name,
                                MessageType.Warning));
                    }
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Loading objects", "Objects have not been loaded.", Name, e, false));
            }
        }

        /// <summary>
        /// Notifies when objects collection updated.
        /// </summary>
        protected virtual void OnObjectsUpdated()
        {
            ObjectsUpdated?.Invoke(this, System.EventArgs.Empty);
        }

        /// <summary>
        /// Subscribes objects event.
        /// </summary>
        private void SubscribeEvents()
        {
            try
            {
                if (Objects == null) return;

                foreach (var obj in Objects)
                {
                    obj.MessageReceived += OnObjectMessageReceived;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Subscribing events", "Error subscribing objects events.", Name, e, false));
            }
        }

        /// <summary>
        /// Unsubscribes objects event.
        /// </summary>
        private void UnsubscribeEvents()
        {
            try
            {
                if (Objects == null) return;

                foreach (var obj in Objects)
                {
                    obj.MessageReceived -= OnObjectMessageReceived;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Subscribing events", "Error subscribing objects events.", Name, e, false));
            }
        }

        /// <summary>
        /// Notifies when object's message received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private void OnObjectMessageReceived(object sender, IMessage e)
        {
            OnMessageReceived(this, e);
        }
    }
}