using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Service.Native
{
    /// <summary>
    /// Service for loading native libraries.
    /// </summary>
    [Export(typeof(IWavesService))]
    public class Service : WavesService, INativeLibraryService
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        private readonly string _defaultNativeDirectory = Path.Combine(Environment.CurrentDirectory, "native");

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("D997C428-571F-4CEE-97B3-5AB60DF8BAAB");

        /// <inheritdoc />
        public override string Name { get; set; } = "Native library loader service";

        /// <inheritdoc />
        [Reactive]
        public List<string> Paths { get; } = new List<string>();

        /// <inheritdoc />
        [Reactive]
        public List<string> Names { get; } = new List<string>();

        /// <inheritdoc />
        public override void Initialize(IWavesCore core)
        {
            if (IsInitialized) return;

            Core = core;

            try
            {
                Update();

                IsInitialized = true;

                OnMessageReceived(this,
                    new WavesMessage(
                        "Initialization",
                        "Service has been initialized.",
                        Name,
                        WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage(
                        "Service initialization", 
                        "Error service initialization.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            try
            {
                Paths.AddRange(LoadConfigurationValue(Core.Configuration, Name + "-Paths", new List<string>()));

                OnMessageReceived(this, 
                    new WavesMessage(
                    "Loading configuration", 
                    "Configuration loaded successfully.", 
                    Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage(
                        "Loading configuration", 
                        "Error loading configuration.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration()
        {
            try
            {
                if (Paths.Count > 0)
                {
                    Core.Configuration.SetPropertyValue(Name + "-Paths", Paths);

                    OnMessageReceived(this, new WavesMessage("Saving configuration", "Configuration saved successfully.",
                        Name,
                        WavesMessageType.Success));
                }

                OnMessageReceived(this, new WavesMessage("Saving configuration", "There is nothing to save.",
                    Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Saving configuration", "Error saving configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void AddPath(string path)
        {
            try
            {
                if (!Paths.Contains(path)) Paths?.Add(path);

                OnMessageReceived(this, new WavesMessage("Adding path", "Path added successfully.",
                    Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Adding path", "Path has not been added.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void RemovePath(string path)
        {
            try
            {
                if (Paths.Contains(path)) Paths?.Remove(path);

                OnMessageReceived(this, new WavesMessage("Removing path",
                    "Path removed successfully.", Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Removing path", "Path has not been removed.", Name, e,
                        false));
            }
        }


        /// <inheritdoc />
        public void Update()
        {
            var defaultDirectory = Path.Combine(_currentDirectory, "native");
            var runtimesDirectory = Path.Combine(_currentDirectory, "runtimes");

            try
            {
                if (!Directory.Exists(defaultDirectory))
                    Directory.CreateDirectory(defaultDirectory);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new WavesMessage(e, false));
            }

            try
            {
                Names.Clear();

                Paths.Add(defaultDirectory);
                Paths.Add(runtimesDirectory);

                foreach (var path in Paths)
                {
                    // var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                    //     .Where(x => !x.EndsWith(".dll"));

                    var files = new List<string>();
                    
                    files.AddRange(SearchFiles(path, ".dll"));
                    files.AddRange(SearchFiles(path, ".so"));
                    files.AddRange(SearchFiles(path, ".dylib"));

                    foreach (var file in files)
                    {
                        if (!File.Exists(file))
                            continue;
                        
                        var info = new FileInfo(file);
                        
                        try
                        {
                            var fileName = info.FullName;
                            
                            var result = NativeLibrary.Load(fileName);

                            if (result == IntPtr.Zero)
                            {
                                var lastError = Marshal.GetLastWin32Error();
                                var error = new Win32Exception(lastError);
                                throw error;
                            }

                            OnMessageReceived(this,
                                new WavesMessage(
                                    "Loading native library",
                                    "Native library " + info.Name + " has been loaded.",
                                    Name,
                                    WavesMessageType.Information));

                            Names.Add(info.FullName);
                        }
                        catch (Exception)
                        {
                            OnMessageReceived(this,
                                new WavesMessage(
                                    "Native library loading error",
                                    "Library " + info.FullName + " can't be loaded on current system.",
                                    Name,
                                    WavesMessageType.Error));
                        }
                    }
                }

                Paths.Remove(defaultDirectory);
                Paths.Remove(runtimesDirectory);
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Loading native libraries", "Native libraries have not been loaded.", Name, e, false));
            }
        }
        
        /// <inheritdoc />
        public override void Dispose()
        {
        }

        /// <summary>
        /// Searches files in current path within all directories by current exntesion.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Returns files collection.</returns>
        private IEnumerable<string> SearchFiles(string path, string extension)
        {
            return Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(x => !x.EndsWith(extension));;
        }
    }
}
