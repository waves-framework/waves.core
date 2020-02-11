using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Fluid.Core.Base;
using Fluid.Core.Enums;
using Fluid.Core.Interfaces;
using Fluid.Core.Native;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    public class ModuleService : Service, IModuleService
    {
        private IEnumerable<IModuleLibrary> _libraries;

        private List<string> _nativeLibrariesNames = new List<string>();
        private List<string> _modulesPaths = new List<string>();
        private List<string> _nativeLibrariesPaths = new List<string>();

        private List<IModule> _modules = new List<IModule>();
        private readonly List<IModule> _clonedModules = new List<IModule>();

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("F21B05E5-6648-448E-9AC9-C7D06A79D346");

        /// <inheritdoc />
        public override string Name { get; set; } = "Module Loader Service";

        /// <inheritdoc />
        public List<string> ModulesPaths
        {
            get => _modulesPaths;
            private set
            {
                if (Equals(value, _modulesPaths)) return;

                _modulesPaths = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public List<string> NativeLibrariesPaths
        {
            get => _nativeLibrariesPaths;
            private set
            {
                if (Equals(value, _nativeLibrariesPaths)) return;

                _nativeLibrariesPaths = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        [ImportMany]
        public IEnumerable<IModuleLibrary> Libraries
        {
            get => _libraries;
            set
            {
                _libraries = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public List<IModule> Modules
        {
            get => _modules;
            set
            {
                if (Equals(value, _modules)) return;

                _modules = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public List<string> NativeLibrariesNames
        {
            get => _nativeLibrariesNames;
            private set
            {
                _nativeLibrariesNames = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IModule GetModule(string id)
        {
            foreach (var module in Modules)
                if (module.Id.ToString().ToUpper().Equals(id.ToUpper()))
                {
                    var clone = (IModule) module.Clone();

                    clone.MessageReceived += OnMessageReceived;

                    _clonedModules.Add(clone);

                    return clone;
                }

            return null;
        }

        /// <inheritdoc />
        public void AddModulePath(string path)
        {
            if (!ModulesPaths.Contains(path)) ModulesPaths.Add(path);
        }

        /// <inheritdoc />
        public void AddNativeLibraryPath(string path)
        {
            if (!NativeLibrariesPaths.Contains(path)) NativeLibrariesPaths.Add(path);
        }

        /// <inheritdoc />
        public void RemoveModulePath(string path)
        {
            if (ModulesPaths.Contains(path)) ModulesPaths.Remove(path);
        }

        /// <inheritdoc />
        public void RemoveNativeLibraryPath(string path)
        {
            if (NativeLibrariesPaths.Contains(path)) NativeLibrariesPaths.Remove(path);
        }

        /// <inheritdoc />
        public void UpdateLibraries()
        {
            UpdateNativeLibraries();
            UpdateMefLibraries();
            UpdateModules();
        }

        /// <inheritdoc />
        public event EventHandler ModulesUpdated;

        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized) return;

            IsInitialized = true;

            OnMessageReceived(this,
                new Message(
                    "Информация",
                    "Сервис инициализирован.",
                    Name,
                    MessageType.Information));
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            ModulesPaths.AddRange(LoadConfigurationValue<List<string>>(configuration, "ModuleService-ModulesPaths"));
            NativeLibrariesPaths.AddRange(
                LoadConfigurationValue<List<string>>(configuration, "ModuleService-NativeLibrariesPaths"));
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            configuration.SetPropertyValue("ModuleService-ModulesPaths",
                ModulesPaths.GetRange(1, ModulesPaths.Count - 1));
            configuration.SetPropertyValue("ModuleService-NativeLibrariesPaths",
                NativeLibrariesPaths.GetRange(1, NativeLibrariesPaths.Count - 1));
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            foreach (var module in Modules)
                module.Dispose();

            foreach (var module in _clonedModules)
                module.Dispose();
        }

        /// <summary>
        ///     Инициализация подключения библиотек по стандарту MEF
        /// </summary>
        private void UpdateMefLibraries()
        {
            var assemblies = new List<Assembly>();

            foreach (var path in ModulesPaths)
            {
                if (!Directory.Exists(path))
                {
                    OnMessageReceived(this,
                        new Message(
                            "Ошибка пути",
                            "Путь к модулям " + path + " не существует или был удален.",
                            Name,
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
            Libraries = container.GetExports<IModuleLibrary>();
        }

        /// <summary>
        ///     Иницилилазиция нативных библиотек
        /// </summary>
        private void UpdateNativeLibraries()
        {
            NativeLibrariesNames.Clear();

            foreach (var path in NativeLibrariesPaths)
            {
                var info = new DirectoryInfo(path);
                var files = info.GetFiles();

                foreach (var file in files)
                {
                    if (file.Extension != ".dll") continue;

                    try
                    {
                        var fileName = file.FullName;

                        var result = Kernel32.LoadLibrary(fileName);

                        if (result == IntPtr.Zero)
                        {
                            var lastError = Marshal.GetLastWin32Error();
                            var error = new Win32Exception(lastError);
                            throw error;
                        }

                        NativeLibrariesNames.Add(file.FullName);
                    }
                    catch (Exception)
                    {
                        OnMessageReceived(this, new Message(
                            "Ошибка загрузки библиотеки",
                            "Библиотека " + file.Name + " не поддерживается данной системой.",
                            Name,
                            MessageType.Error));
                    }
                }
            }
        }

        /// <summary>
        ///     Инициализация модулей
        /// </summary>
        private void UpdateModules()
        {
            if (Libraries == null)
                return;

            Modules.Clear();

            foreach (var library in Libraries)
            {
                library.UpdateModulesList();

                foreach (var module in library.Modules)
                {
                    Modules.Add(module);

                    module.MessageReceived += OnMessageReceived;

                    module.Initialize();
                }
            }

            OnModulesUpdated();
        }

        /// <summary>
        ///     Уведомление об обновлении списка модулей.
        /// </summary>
        protected virtual void OnModulesUpdated()
        {
            ModulesUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
}