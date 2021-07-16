using System;
using System.Linq;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base.Extensions
{
    /// <summary>
    /// Extensions for configurable objects.
    /// </summary>
    public static class ConfigurableExtensions
    {
        /// <summary>
        /// Gets name of plugin.
        /// </summary>
        /// <param name="obj">Configurable.</param>
        /// <returns>Name.</returns>
        public static string GetPluginTypeName(this IWavesConfigurableObject obj)
        {
            var attribute = obj.GetType().GetCustomAttributes(true).Where(x => x is WavesPluginAttribute);
            var list = attribute.ToList();
            if (!(list.FirstOrDefault() is WavesPluginAttribute pluginAttribute))
            {
                return "Unknown";
            }

            return pluginAttribute.Type.Name;
        }

        /// <summary>
        /// Gets configurable ID.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>GUID.</returns>
        public static Guid GetGuid(this IWavesConfigurableObject obj)
        {
            var attribute = obj.GetType().GetCustomAttributes(true).Where(x => x is WavesObjectAttribute);
            var list = attribute.ToList();
            if (!(list.FirstOrDefault() is WavesObjectAttribute objectAttribute))
            {
                return Guid.Empty;
            }

            return objectAttribute.Id;
        }
    }
}
