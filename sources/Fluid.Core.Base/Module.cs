using System;
using System.Collections.Generic;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Module base class.
    /// </summary>
    [Serializable]
    public abstract class Module : Object, IModule
    {
        /// <inheritdoc />
        public bool IsInitialized { get; set; }

        /// <inheritdoc />
        public abstract string Icon { get; }

        /// <inheritdoc />
        public abstract override string Name { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public abstract override Guid Id { get; }

        /// <inheritdoc />
        public abstract string Manufacturer { get; }

        /// <inheritdoc />
        public abstract Version Version { get; }

        /// <inheritdoc />
        public IConfiguration Configuration { get; set; } = new Configuration();

        /// <inheritdoc />
        public ICollection<IEntryPoint> Inputs { get; set; } = new List<IEntryPoint>();

        /// <inheritdoc />
        public ICollection<IEntryPoint> Outputs { get; set; } = new List<IEntryPoint>();

        /// <inheritdoc />
        public abstract void Initialize();

        /// <inheritdoc />
        public abstract object Clone();

        /// <inheritdoc />
        public abstract void Dispose();
    }
}