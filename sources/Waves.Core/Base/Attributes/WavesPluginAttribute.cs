using System;
using System.Runtime.CompilerServices;

namespace Waves.Core.Base.Attributes
{
    /// <summary>
    ///     Attribute for plugin.
    /// </summary>
    public class WavesPluginAttribute : WavesObjectAttribute
    {
        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="isSingleInstance">Whether plugin must has single instance when registering in container.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesPluginAttribute(
            Type pluginType,
            bool isSingleInstance = false,
            [CallerMemberName] string name = default)
            : base(name)
        {
            Type = pluginType;
            IsSingleInstance = isSingleInstance;
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="key">Registration key.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="isSingleInstance">Whether plugin must has single instance when registering in container.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesPluginAttribute(
            object key,
            Type pluginType,
            bool isSingleInstance = false,
            [CallerMemberName] string name = default)
            : base(name)
        {
            Key = key;
            Type = pluginType;
            IsSingleInstance = isSingleInstance;
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="id">Id of plugin.</param>
        /// <param name="key">Registration key.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="isSingleInstance">Whether plugin must has single instance when registering in container.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesPluginAttribute(
            Guid id,
            object key,
            Type pluginType,
            bool isSingleInstance = false,
            [CallerMemberName] string name = default)
            : base(id, name)
        {
            Key = key;
            Type = pluginType;
            IsSingleInstance = isSingleInstance;
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesPluginAttribute" />.
        /// </summary>
        /// <param name="id">Id of plugin.</param>
        /// <param name="key">Registration key.</param>
        /// <param name="pluginType">Plugin type.</param>
        /// <param name="isSingleInstance">Whether plugin must has single instance when registering in container.</param>
        /// <param name="name">Name of plugin.</param>
        public WavesPluginAttribute(
            string id,
            object key,
            Type pluginType,
            bool isSingleInstance = false,
            [CallerMemberName] string name = default)
            : base(id, name)
        {
            Key = key;
            Type = pluginType;
            IsSingleInstance = isSingleInstance;
        }

        /// <summary>
        ///     Gets whether plugin must has single instance when registering in container.
        /// </summary>
        public bool IsSingleInstance { get; }

        /// <summary>
        ///     Gets key.
        /// </summary>
        public object Key { get; }

        /// <summary>
        ///     Gets plugin type.
        /// </summary>
        public Type Type { get; }
    }
}
