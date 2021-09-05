using System;

namespace Waves.Core.Base
{
    /// <summary>
    /// Registration model.
    /// </summary>
    public class WavesContainerRegistration : WavesObject
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesContainerRegistration"/>.
        /// </summary>
        /// <param name="obj">Registered object.</param>
        /// <param name="isSingleInstance">Whether registered is single instance.</param>
        /// <param name="key">Object key.</param>
        public WavesContainerRegistration(
            object obj,
            bool isSingleInstance = false,
            object key = null)
        {
            Object = obj;
            Key = key;
            IsSingleInstance = isSingleInstance;
        }

        /// <summary>
        /// Gets whether registration made as single instance.
        /// </summary>
        public bool IsSingleInstance { get; }

        /// <summary>
        /// Gets whether is type registration.
        /// </summary>
        public bool IsType => Object is Type;

        /// <summary>
        /// Gets whether registration has key.
        /// </summary>
        public bool HasKey => Key != null;

        /// <summary>
        /// Gets object.
        /// </summary>
        public object Object { get; }

        /// <summary>
        /// Gets key.
        /// </summary>
        public object Key { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Key != null ? $"{Object} ({Key})" : Object.ToString();
        }
    }
}
