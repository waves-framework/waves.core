using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Extensions;
using Waves.Core.Base.Interfaces;
using Waves.Core.Plugins.Services.Interfaces;
using Waves.Core.Utils.Serialization;

namespace Waves.Core.Plugins.Services
{
    /// <summary>
    /// Configuration service.
    /// </summary>
    [WavesService(
        "9B5B2A80-DD6A-4D6E-ABCB-5C0C6AD40E09",
        typeof(IWavesConfigurationService))]
    public class WavesConfigurationService : WavesService, IWavesConfigurationService
    {
        private readonly IWavesContainerService _containerService;
        private readonly IWavesTypeLoaderService _typeLoaderService;
        private readonly IWavesCore _core;

        private ICollection<Type> _loadedTypes;
        private Dictionary<Guid, IWavesConfigurableObject> _configurableObjects;

        /// <summary>
        /// Creates new instance of <see cref="WavesConfigurationService"/>.
        /// </summary>
        /// <param name="containerService">Container service.</param>
        /// <param name="typeLoaderService">Type loader service.</param>
        /// <param name="core">Core.</param>
        public WavesConfigurationService(
            IWavesContainerService containerService,
            IWavesTypeLoaderService typeLoaderService,
            IWavesCore core)
        {
            _containerService = containerService;
            _typeLoaderService = typeLoaderService;
            _core = core;
        }

        /// <inheritdoc />
        public string Path { get; set; } = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? throw new InvalidOperationException(), "config");

        /// <inheritdoc />
        public IDictionary<Guid, IWavesConfiguration> Configurations { get; private set; }

        /// <inheritdoc />
        public override async Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return;
            }

            _loadedTypes = _typeLoaderService.Types;

            await LoadTypesAsync();
            await CheckPathAsync();
            await LoadConfigurationsAsync();
        }

        /// <inheritdoc />
        public async Task LoadConfigurationsAsync()
        {
            Configurations = new Dictionary<Guid, IWavesConfiguration>();

            foreach (var path in Directory.GetFiles(Path))
            {
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Extension.Equals(".config"))
                {
                    continue;
                }

                var id = Guid.Parse(fileInfo.Name.Replace(fileInfo.Extension, string.Empty));
                var config = Json.ReadFile<WavesConfiguration>(path);
                Configurations.Add(id, config);

                await _core.WriteLogAsync(new WavesTextMessage(
                    $"Configuration file ({id}) loaded.",
                    "Configuration",
                    this));
            }
        }

        /// <inheritdoc />
        public async Task SaveConfigurationsAsync()
        {
            Configurations = new Dictionary<Guid, IWavesConfiguration>();

            foreach (var pair in _configurableObjects)
            {
                var id = pair.Key;
                var configurable = pair.Value;

                await configurable.SaveConfigurationAsync();

                if (configurable.Configuration == null)
                {
                    await _core.WriteLogAsync(new WavesTextMessage(
                        $"Configuration for configurable object {configurable.GetPluginTypeName()})not initialized.",
                        "Configuration",
                        this,
                        WavesMessageType.Warning));

                    continue;
                }

                if (!(configurable.Configuration?.GetPropertiesCount() > 0))
                {
                    continue;
                }

                Json.WriteToFile(await GetConfigNameAsync(id), configurable.Configuration);

                await _core.WriteLogAsync(new WavesTextMessage(
                    $"Configuration for configurable object {configurable.GetPluginTypeName()} saved.",
                    "Configuration",
                    this,
                    WavesMessageType.Information));
            }
        }

        /// <inheritdoc />
        public Task AddConfigurableAsync(IWavesConfigurableObject configurable)
        {
            var id = configurable.GetGuid();
            _configurableObjects.Add(id, configurable);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RemoveConfigurableAsync(IWavesConfigurableObject configurable)
        {
            var id = configurable.GetGuid();
            _configurableObjects.Remove(id);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<IWavesConfiguration> GetConfigurationAsync(IWavesConfigurableObject configurable)
        {
            var id = configurable.GetGuid();
            return Task.FromResult(Configurations.ContainsKey(id) ? Configurations[id] : null);
        }

        /// <inheritdoc />
        public Task ImportConfigurationsAsync(string fileName)
        {
            // TODO: Importing configurations.
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ExportConfigurationsAsync(string fileName)
        {
            // TODO: Exporting configurations.
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "JSON Configuration Service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _configurableObjects.Clear();
            Configurations.Clear();

            // TODO: your code for release unmanaged resources.
        }

        /// <summary>
        /// Load types.
        /// </summary>
        private async Task LoadTypesAsync()
        {
            _configurableObjects = new Dictionary<Guid, IWavesConfigurableObject>();

            foreach (var type in _loadedTypes)
            {
                try
                {
                    var attribute = type.GetCustomAttributes(true).Where(x => x is WavesPluginAttribute);
                    var list = attribute.ToList();
                    if (!(list.FirstOrDefault() is WavesPluginAttribute pluginAttribute))
                    {
                        continue;
                    }

                    if (!type.GetInterfaces().Contains(typeof(IWavesConfigurableObject)))
                    {
                        continue;
                    }

                    var instance = await _containerService.GetInstanceAsync(pluginAttribute.Type);
                    _configurableObjects.Add(pluginAttribute.Id, (IWavesConfigurableObject)instance);
                }
                catch (Exception e)
                {
                    await _core.WriteLogAsync(e, this);
                }
            }
        }

        /// <summary>
        /// Check path whether it is created.
        /// </summary>
        private Task CheckPathAsync()
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets name of config.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Name of config.</returns>
        private Task<string> GetConfigNameAsync(Guid id)
        {
            return Task.FromResult(System.IO.Path.Combine(Path, $"{id}.config"));
        }
    }
}
