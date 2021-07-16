using System;
using System.Runtime.CompilerServices;

namespace Waves.Core.Base.Attributes
{
    /// <summary>
    ///     Attribute for services.
    /// </summary>
    public class WavesServiceAttribute : WavesPluginAttribute
    {
        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="pluginType">Plugin type.</param>
        public WavesServiceAttribute(
            Type pluginType)
            : base(
                null,
                pluginType,
                true)
        {
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="name">Name of service.</param>
        public WavesServiceAttribute(
            Type pluginType,
            [CallerMemberName] string name = default)
            : base(
                name,
                null,
                pluginType,
                true)
        {
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="id">Id of plugin.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesServiceAttribute(
            Guid id,
            Type pluginType,
            [CallerMemberName] string name = default)
            : base(
                id,
                null,
                pluginType,
                true,
                name)
        {
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="id">Id of plugin.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesServiceAttribute(
            string id,
            Type pluginType,
            [CallerMemberName] string name = default)
            : base(
                id,
                null,
                pluginType,
                true,
                name)
        {
        }
    }
}
