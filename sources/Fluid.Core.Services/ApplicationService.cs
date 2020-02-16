using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    public class ApplicationService : Service, IApplicationService
    {
        private List<string> _paths = new List<string>();
        private IEnumerable<IApplication> _applications;
        private ICollection<IApplicationAction> _applicationActions = new List<IApplicationAction>();

        /// <inheritdoc />
        public List<string> Paths
        {
            get => _paths;
            private set
            {
                if (Equals(value, _paths)) return;
                _paths = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IEnumerable<IApplication> Applications
        {
            get => _applications;
            private set
            {
                if (Equals(value, _applications)) return;
                _applications = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ICollection<IApplicationAction> ApplicationActions
        {
            get => _applicationActions;
            private set
            {
                if (Equals(value, _applicationActions)) return;
                _applicationActions = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("B709823E-22BE-4297-982B-59A90B82D977");

        /// <inheritdoc />
        public override string Name { get; set; } = "Application Loader Service";

        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized) return;

            LoadApplications();

            IsInitialized = true;

            OnMessageReceived(this,
                new Message(
                    "Информация",
                    "Сервис инициализирован.",
                    nameof(ApplicationService),
                    MessageType.Information));
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            Paths.AddRange(LoadConfigurationValue<List<string>>(configuration, "ApplicationService-Paths"));
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            configuration.SetPropertyValue("ApplicationService-Paths", Paths.GetRange(1, Paths.Count - 1));
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (Applications == null) return;

            foreach (var application in Applications)
                application.Dispose();

            UnsubscribeApplicationCollectionEvents();
        }

        /// <inheritdoc />
        public event EventHandler ApplicationsUpdated;

        /// <inheritdoc />
        public event EventHandler ApplicationsActionsUpdated;

        /// <inheritdoc />
        public void AddPath(string path)
        {
            if (!Paths.Contains(path)) Paths?.Add(path);
        }

        /// <inheritdoc />
        public void RemovePath(string path)
        {
            if (Paths.Contains(path)) Paths?.Remove(path);
        }

        /// <inheritdoc />
        public void UpdateApplications()
        {
            LoadApplications();
        }

        /// <summary>
        ///     Загружает модули.
        /// </summary>
        private void LoadApplications()
        {
            UnsubscribeApplicationCollectionEvents();

            var assemblies = new List<Assembly>();

            foreach (var path in Paths)
            {
                if (!Directory.Exists(path))
                {
                    OnMessageReceived(this,
                        new Message(
                            "Ошибка пути",
                            "Путь к приложению " + path + " не существует или был удален.",
                            "Core",
                            MessageType.Error));

                    continue;
                }

                foreach (var file in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
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
            }

            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            using var container = configuration.CreateContainer();
            Applications = container.GetExports<IApplication>();

            SubscribeApplicationEvents();

            OnApplicationsUpdated();
        }

        /// <summary>
        ///     Подписка на события приложения.
        /// </summary>
        private void SubscribeApplicationEvents()
        {
            foreach (var application in Applications)
            {
                application.ActionsUpdated += OnApplicationActionsUpdated;
                application.MessageReceived += OnApplicationMessageReceived;
            }
        }

        /// <summary>
        ///     Отписка от событий приложения.
        /// </summary>
        private void UnsubscribeApplicationCollectionEvents()
        {
            if (Applications == null) return;

            foreach (var application in Applications)
            {
                application.ActionsUpdated -= OnApplicationActionsUpdated;
                application.MessageReceived -= OnApplicationMessageReceived;
            }
        }

        /// <summary>
        ///     Действия при приеме системного сообщения от приложений.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationMessageReceived(object sender, IMessage e)
        {
            OnMessageReceived(sender, e);
        }

        /// <summary>
        ///     Действия при обновлении коллекции действий приложения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationActionsUpdated(object sender, EventArgs e)
        {
            OnApplicationsActionsUpdated();
        }

        /// <summary>
        ///     Уведомление об обновлении коллекции приложений.
        /// </summary>
        protected virtual void OnApplicationsUpdated()
        {
            ApplicationsUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Уведомление об обновлении коллекции действий приложений.
        /// </summary>
        protected virtual void OnApplicationsActionsUpdated()
        {
            ApplicationsActionsUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}