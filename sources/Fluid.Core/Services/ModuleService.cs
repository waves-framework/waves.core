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
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Native.Windows;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    /// <summary>
    ///     Module service.
    /// </summary>
    [Export(typeof(IService))]
    public class ModuleService : Service, IModuleService
    {
        private readonly List<IModule> _clonedModules = new List<IModule>();
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("F21B05E5-6648-448E-9AC9-C7D06A79D346");

        /// <inheritdoc />
        public override string Name { get; set; } = "Module Loader Service";

        /// <inheritdoc />
        public List<string> ModulesPaths { get; } = new List<string>();

        /// <inheritdoc />
        public List<string> NativeLibrariesPaths { get; } = new List<string>();

        /// <inheritdoc />
        [ImportMany]
        public IEnumerable<IModuleLibrary> Libraries { get; private set; }

        /// <inheritdoc />
        public List<IModule> Modules { get; } = new List<IModule>();

        /// <inheritdoc />
        public List<string> NativeLibrariesNames { get; } = new List<string>();

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

            UpdateLibraries();

            OnMessageReceived(this,
                new Message("Initialization", "Service was initialized.", Name, MessageType.Information));
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            ModulesPaths.AddRange(LoadConfigurationValue(configuration, "ModuleService-ModulesPaths",
                new List<string>()));
            NativeLibrariesPaths.AddRange(LoadConfigurationValue(configuration, "ModuleService-NativeLibrariesPaths",
                new List<string>()));
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            if (ModulesPaths.Count > 1)
                configuration.SetPropertyValue("ModuleService-ModulesPaths",
                    ModulesPaths.GetRange(1, ModulesPaths.Count - 1));

            if (NativeLibrariesPaths.Count > 1)
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
        ///     Initialized MEF Libraries.
        /// </summary>
        private void UpdateMefLibraries()
        {
            var defaultDirectory = Path.Combine(_currentDirectory, "modules");

            if (!Directory.Exists(defaultDirectory))
                Directory.CreateDirectory(defaultDirectory);

            var assemblies = new List<Assembly>();

            foreach (var path in ModulesPaths)
            {
                if (!Directory.Exists(path))
                {
                    OnMessageReceived(this,
                        new Message(
                            "Loading path error",
                            "Path to application ( " + path + ") doesn't exists or was deleted.",
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
        ///     Initializes native libraries.
        /// </summary>
        private void UpdateNativeLibraries()
        {
            var defaultDirectory = Path.Combine(_currentDirectory, "native");

            if (!Directory.Exists(defaultDirectory))
                Directory.CreateDirectory(defaultDirectory);

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
                        OnMessageReceived(this,
                            new Message(
                                "Native library loading error",
                                "Library " + file.Name + " can't be loaded on current system.",
                                Name,
                                MessageType.Error));
                    }
                }
            }
        }

        /// <summary>
        ///     Update modules collection.
        /// </summary>
        private void UpdateModules()
        {
            if (Libraries == null)
                return;

            Modules.Clear();

            foreach (var library in Libraries)
            {
                library.UpdateModulesCollection();

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
        ///     Notifies when modules collection updated.
        /// </summary>
        protected virtual void OnModulesUpdated()
        {
            ModulesUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}