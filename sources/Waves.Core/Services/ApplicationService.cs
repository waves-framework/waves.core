using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Services.Interfaces;

namespace Waves.Core.Services
{
    /// <summary>
    ///     Application service.
    /// </summary>
    [Export(typeof(IService))]
    public class ApplicationService : Service, IApplicationService
    {
        /// <inheritdoc />
        public event EventHandler ApplicationsUpdated;

        /// <inheritdoc />
        public event EventHandler ApplicationsActionsUpdated;

        /// <inheritdoc />
        public List<string> Paths { get; set; } = new List<string>();

        /// <inheritdoc />
        [ImportMany]
        public IEnumerable<IApplication> Applications { get; private set; }

        /// <inheritdoc />
        public ICollection<IApplicationAction> ApplicationActions { get; } = new List<IApplicationAction>();

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("B709823E-22BE-4297-982B-59A90B82D977");

        /// <inheritdoc />
        public override string Name { get; set; } = "Application Loader Service";

        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized) return;

            try
            {
                LoadApplications();

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
                Paths.AddRange(LoadConfigurationValue(configuration, "ApplicationService-Paths", new List<string>()));

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
                    configuration.SetPropertyValue("ApplicationService-Paths", Paths);

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
        public override void Dispose()
        {
            if (Applications == null) return;

            try
            {
                foreach (var application in Applications)
                    application.Dispose();

                UnsubscribeApplicationCollectionEvents();
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Service disposing", "Error service disposing.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void AddPath(string path)
        {
            try
            {
                if (!Paths.Contains(path)) Paths?.Add(path);

                OnMessageReceived(this, new Message("Adding applications path", "Applications path added successfully.",
                    Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Adding applications path", "Applications path has not been added.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void RemovePath(string path)
        {
            try
            {
                if (Paths.Contains(path)) Paths?.Remove(path);

                OnMessageReceived(this, new Message("Removing applications path",
                    "Applications path removed successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Removing applications path", "Applications path has not been removed.", Name, e,
                        false));
            }
        }

        /// <inheritdoc />
        public void UpdateApplicationsCollection()
        {
            try
            {
                LoadApplications();
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Updating applications collection", "Applications collection has not been updated.",
                        Name, e, false));
            }
        }

        /// <summary>
        ///     Notifies when applications actions updated.
        /// </summary>
        protected virtual void OnApplicationsActionsUpdated()
        {
            OnMessageReceived(this, new Message("Applications collection", "Applications collections updated.", Name,
                MessageType.Information));

            ApplicationsActionsUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Loads applications.
        /// </summary>
        private void LoadApplications()
        {
            try
            {
                UnsubscribeApplicationCollectionEvents();

                var assemblies = new List<Assembly>();

                foreach (var path in Paths)
                    Extensions.Assembly.GetAssemblies(assemblies, path);

                var configuration = new ContainerConfiguration().WithAssemblies(assemblies);

                using var container = configuration.CreateContainer();
                Applications = container.GetExports<IApplication>();

                SubscribeApplicationEvents();

                OnApplicationsUpdated();

                if (Applications != null)
                {
                    var applications = Applications as IApplication[] ?? Applications.ToArray();

                    if (!applications.Any())
                        OnMessageReceived(this,
                            new Message("Loading applications", "Applications not found.", Name, MessageType.Warning));
                    else
                        OnMessageReceived(this, new Message("Loading applications",
                            "Applications loads successfully (" + applications.Count() + " applications).", Name,
                            MessageType.Success));
                }
                else
                {
                    OnMessageReceived(this,
                        new Message("Loading applications", "Applications were not loaded.", Name,
                            MessageType.Warning));
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Loading applications", "Applications have not been loaded.", Name, e, false));
            }
        }

        /// <summary>
        ///     Subscribes for application events.
        /// </summary>
        private void SubscribeApplicationEvents()
        {
            try
            {
                foreach (var application in Applications)
                {
                    application.ActionsUpdated += OnApplicationActionsUpdated;
                    application.MessageReceived += OnApplicationMessageReceived;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Subscribing events", "Error subscribing application events.", Name, e, false));
            }
        }

        /// <summary>
        ///     Unsubscribe to application events.
        /// </summary>
        private void UnsubscribeApplicationCollectionEvents()
        {
            if (Applications == null) return;

            try
            {
                foreach (var application in Applications)
                {
                    application.ActionsUpdated -= OnApplicationActionsUpdated;
                    application.MessageReceived -= OnApplicationMessageReceived;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Unsubscribing events", "Error unsubscribing application events.", Name, e, false));
            }
        }

        /// <summary>
        ///     Notifies when application receive message.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Message.</param>
        private void OnApplicationMessageReceived(object sender, IMessage e)
        {
            OnMessageReceived(sender, e);
        }

        /// <summary>
        ///     Notifies when application actions updated.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Message.</param>
        private void OnApplicationActionsUpdated(object sender, EventArgs e)
        {
            OnApplicationsActionsUpdated();
        }

        /// <summary>
        ///     Notifies when applications collection updated.
        /// </summary>
        protected virtual void OnApplicationsUpdated()
        {
            ApplicationsUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}