using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Gets collection of libraries.
        /// </summary>
        [ImportMany]
        private IEnumerable<IModuleLibrary> Libraries { get; set; }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("F21B05E5-6648-448E-9AC9-C7D06A79D346");

        /// <inheritdoc />
        public override string Name { get; set; } = "Module Loader Service";

        /// <inheritdoc />
        public ICollection<string> ModulesPaths { get; } = new ObservableCollection<string>();

        /// <inheritdoc />
        public ICollection<string> NativeLibrariesPaths { get; } = new ObservableCollection<string>();

        /// <inheritdoc />
        public ICollection<IModule> Modules { get; } = new ObservableCollection<IModule>();

        /// <inheritdoc />
        public ICollection<string> NativeLibrariesNames { get; } = new ObservableCollection<string>();

        /// <inheritdoc />
        public IModule GetModule(string id)
        {
            try
            {
                foreach (var module in Modules)
                {
                    if (module.Id.ToString().ToUpper().Equals(id.ToUpper()))
                    {
                        var clone = (IModule)module.Clone();

                        clone.MessageReceived += OnMessageReceived;

                        _clonedModules.Add(clone);

                        OnMessageReceived(this,
                            new Message(
                                "Getting module",
                                "Module " + module.Name + " has been cloned and registered.",
                                Name,
                                MessageType.Information));

                        return clone;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Getting module", "Error getting module.", Name, e, false));

                return null;
            }
        }

        /// <inheritdoc />
        public void AddModulePath(string path)
        {
            try
            {
                if (!ModulesPaths.Contains(path)) ModulesPaths.Add(path);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Adding modules path", "Modules path has not been added.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void AddNativeLibraryPath(string path)
        {
            try
            {
                if (!NativeLibrariesPaths.Contains(path)) NativeLibrariesPaths.Add(path);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Adding native libraries path", "Native libraries path has not been added.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void RemoveModulePath(string path)
        {
            try
            {
                if (ModulesPaths.Contains(path)) ModulesPaths.Remove(path);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Removing modules path", "Modules path has not been removed.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void RemoveNativeLibraryPath(string path)
        {
            try
            {
                if (NativeLibrariesPaths.Contains(path))
                    NativeLibrariesPaths.Remove(path);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Removing native libraries path", "Native libraries path has not been removed.", Name, e, false));
            }
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

            try
            {
                UpdateLibraries();

                IsInitialized = true;

                OnMessageReceived(this,
                    new Message("Initialization", "Service has been initialized.", Name, MessageType.Information));
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Service initialization", "Error service initialization.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            try
            {
                ModulesPaths.Clear();
                NativeLibrariesPaths.Clear();

                var modulesPaths = LoadConfigurationValue(configuration, "ModuleService-ModulesPaths", new List<string>());
                var nativeLibrariesPaths = LoadConfigurationValue(configuration, "ModuleService-NativeLibrariesPaths", new List<string>());

                foreach (var path in modulesPaths)
                    ModulesPaths.Add(path);

                foreach (var path in nativeLibrariesPaths)
                    NativeLibrariesPaths.Add(path);

                OnMessageReceived(this, new Message("Configuration loading", "Configuration loads successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Loading configuration", "Error loading configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            try
            {
                if (ModulesPaths.Count > 0)
                {
                    var modulesPaths = new string[ModulesPaths.Count - 1];

                    ModulesPaths.CopyTo(modulesPaths, 1);

                    configuration.SetPropertyValue("ModuleService-ModulesPaths", modulesPaths);
                }

                if (NativeLibrariesPaths.Count > 0)
                {
                    var nativeLibrariesPaths = new string[NativeLibrariesPaths.Count - 1];

                    NativeLibrariesPaths.CopyTo(nativeLibrariesPaths, 1);

                    configuration.SetPropertyValue("ModuleService-NativeLibrariesPaths", nativeLibrariesPaths);
                }

                OnMessageReceived(this, new Message("Configuration saving", "Configuration saves successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Saving configuration", "Error saving configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            try
            {
                foreach (var module in Modules)
                {
                    try
                    {
                        module.Dispose();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error disposing module (" + module.Name + ")", e);
                    }
                }

                foreach (var module in _clonedModules)
                {
                    try
                    {
                        module.Dispose();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error disposing module (" + module.Name + ")", e);
                    }
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Service disposing", "Error service disposing.", Name, e, false));
            }
        }

        /// <summary>
        ///     Notifies when modules collection updated.
        /// </summary>
        protected virtual void OnModulesUpdated()
        {
            ModulesUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Initialized MEF Libraries.
        /// </summary>
        private void UpdateMefLibraries()
        {
            var defaultDirectory = Path.Combine(_currentDirectory, "modules");

            try
            {
                if (!Directory.Exists(defaultDirectory))
                    Directory.CreateDirectory(defaultDirectory);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }

            try
            {
                var assemblies = new List<Assembly>();

                foreach (var path in ModulesPaths)
                    Extensions.Assembly.GetAssemblies(assemblies, path);

                var configuration = new ContainerConfiguration()
                    .WithAssemblies(assemblies);

                using var container = configuration.CreateContainer();
                Libraries = container.GetExports<IModuleLibrary>();

                if (Libraries != null)
                {
                    var moduleLibraries = Libraries as IModuleLibrary[] ?? Libraries.ToArray();

                    if (!moduleLibraries.Any())
                    {
                        OnMessageReceived(this, new Message("Loading module libraries", "Module libraries not found.", Name,
                            MessageType.Warning));
                    }
                    else
                    {
                        OnMessageReceived(this, new Message("Loading module libraries", "Module libraries loads successfully (" + moduleLibraries.Count() + " libraries).", Name,
                            MessageType.Success));
                    }
                }
                else
                {
                    OnMessageReceived(this, new Message("Loading module libraries", "Module libraries were not loaded.", Name, MessageType.Warning));
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Loading module libraries", "Module libraries have not been loaded.", Name, e, false));
            }
        }

        /// <summary>
        ///     Initializes native libraries.
        /// </summary>
        private void UpdateNativeLibraries()
        {
            var defaultDirectory = Path.Combine(_currentDirectory, "native");

            try
            {
                if (!Directory.Exists(defaultDirectory))
                    Directory.CreateDirectory(defaultDirectory);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }

            try
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

                            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                            {
                                var result = Kernel32.LoadLibrary(fileName);

                                if (result == IntPtr.Zero)
                                {
                                    var lastError = Marshal.GetLastWin32Error();
                                    var error = new Win32Exception(lastError);
                                    throw error;
                                }
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
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Loading native libraries", "Native libraries have not been loaded.", Name, e, false));
            }
        }

        /// <summary>
        ///     Update modules collection.
        /// </summary>
        private void UpdateModules()
        {
            if (Libraries == null)
                return;

            try
            {
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
            catch (Exception e)
            {
                OnMessageReceived(this, new Message("Updating modules", "Modules have not been loaded.", Name, e, false));
            }
        }
    }
}