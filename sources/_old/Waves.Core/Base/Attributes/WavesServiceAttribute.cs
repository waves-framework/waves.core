using System;
using Waves.Core.Base.Enums;

namespace Waves.Core.Base.Attributes
{
    /// <summary>
    ///     Attribute for services.
    /// </summary>
    public class WavesServiceAttribute : WavesPluginAttribute
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesServiceAttribute"/>.
        /// </summary>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="lifetimeType">Plugin lifetime type.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesServiceAttribute(
            Type pluginType,
            WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
            string name = default)
            : base(pluginType, lifetimeType, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Registration key.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="lifetimeType">Plugin lifetime type.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesServiceAttribute(
            object key,
            Type pluginType,
            WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
            string name = default)
            : base(key, pluginType, lifetimeType, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id of plugin.</param>
        /// <param name="key">Registration key.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="lifetimeType">Plugin lifetime type.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesServiceAttribute(
            Guid id,
            object key,
            Type pluginType,
            WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
            string name = default)
            : base(id, key, pluginType, lifetimeType, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id of plugin.</param>
        /// <param name="key">Registration key.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="lifetimeType">Plugin lifetime type.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesServiceAttribute(
            string id,
            object key,
            Type pluginType,
            WavesLifetimeType lifetimeType = WavesLifetimeType.Transient,
            string name = default)
            : base(id, key, pluginType, lifetimeType, name)
        {
        }
    }
}
