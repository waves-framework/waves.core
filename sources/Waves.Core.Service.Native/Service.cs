using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.IO;
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
    [Export(typeof(IService))]
    public class Service : Base.Service, INativeLibraryService
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        private readonly string _defaultNativeDirectory = Path.Combine(Environment.CurrentDirectory, "native");

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("D997C428-571F-4CEE-97B3-5AB60DF8BAAB");

        /// <inheritdoc />
        public override string Name { get; set; } = "Native library loader service";

        /// <inheritdoc />
        [Reactive]
        public List<string> Paths { get; protected set; } = new List<string>();

        /// <inheritdoc />
        [Reactive]
        public List<string> Names { get; protected set; } = new List<string>();

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
                        MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Service initialization", "Error service initialization.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            try
            {
                Paths.AddRange(LoadConfigurationValue(Core.Configuration, Name + "-Paths", new List<string>()));

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
        public override void SaveConfiguration()
        {
            try
            {
                if (Paths.Count > 0)
                {
                    Core.Configuration.SetPropertyValue(Name + "-Paths", Paths);

                    OnMessageReceived(this, new Message("Saving configuration", "Configuration saved successfully.",
                        Name,
                        MessageType.Success));
                }

                OnMessageReceived(this, new Message("Saving configuration", "There is nothing to save.",
                    Name,
                    MessageType.Success));
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
        public void Update()
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
                Names.Clear();

                Paths.Add(_defaultNativeDirectory);

                foreach (var path in Paths)
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
                                var result = NativeLibrary.Load(fileName);

                                if (result == IntPtr.Zero)
                                {
                                    var lastError = Marshal.GetLastWin32Error();
                                    var error = new Win32Exception(lastError);
                                    throw error;
                                }

                                OnMessageReceived(this,
                                    new Message(
                                        "Loading native library",
                                        "Native library " + file.Name + " has been loaded.",
                                        Name,
                                        MessageType.Information));
                            }

                            Names.Add(file.FullName);
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

                Paths.Remove(_defaultNativeDirectory);
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Loading native libraries", "Native libraries have not been loaded.", Name, e, false));
            }
        }
        
        /// <inheritdoc />
        public override void Dispose()
        {
        }
    }
}
