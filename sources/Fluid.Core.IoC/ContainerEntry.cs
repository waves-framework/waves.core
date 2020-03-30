using System;
using System.Collections.Generic;

namespace Fluid.Core.IoC
{
    /// <summary>
    /// Container entry.
    /// </summary>
    internal class ContainerEntry : List<Func<SimpleContainer, object>>
    {
        /// <summary>
        /// Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public Type Service { get; set; }
    }
}