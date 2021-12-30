using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Interfaces;
using Waves.Core.Old.Plugins.Services.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    /// Configurable object abstraction.
    /// </summary>
    public abstract class WavesConfigurableObject :
        WavesObject,
        IWavesConfigurableObject
    {
        /// <summary>
        /// Creates new instance os <see cref="WavesConfigurableObject"/>.
        /// </summary>
        /// <param name="configurationService">Instance of configuration service.</param>
        protected WavesConfigurableObject(IWavesConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        /// <inheritdoc />
        [Reactive]
        public IWavesConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets configuration service.
        /// </summary>
        protected IWavesConfigurationService ConfigurationService { get; private set; }

        /// <inheritdoc />
        public async Task LoadConfigurationAsync()
        {
            Configuration = await ConfigurationService.GetConfigurationAsync(this);

            if (Configuration == null)
            {
                return;
            }

            var pluginType = GetType();
            var properties = pluginType.GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is WavesPropertyAttribute)
                    {
                        property.SetValue(this, Configuration.GetPropertyValue(property.Name));
                    }
                }
            }
        }

        /// <inheritdoc />
        public virtual Task SaveConfigurationAsync()
        {
            Configuration = new WavesConfiguration();

            var pluginType = GetType();
            var properties = pluginType.GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (!(attribute is WavesPropertyAttribute))
                    {
                        continue;
                    }

                    var name = property.Name;
                    var value = property.GetValue(this);
                    Configuration.AddProperty(name, value);
                }
            }

            return Task.CompletedTask;
        }
    }
}
