using System;
using Fluid.Core.Interfaces;

namespace Fluid.Core.Base
{
    public abstract class Object : ObservableObject, IObject
    {
        /// <inheritdoc />
        public abstract Guid Id { get; }

        /// <inheritdoc />
        public abstract string Name { get; set; }
    }
}