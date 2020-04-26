using System;

namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of entry point's classes.
    /// </summary>
    public interface IEntryPoint : IObject
    {
        /// <summary>
        ///     Gets whether a point is a property.
        /// </summary>
        bool IsProperty { get; }

        /// <summary>
        ///     Gets current value of point.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///     Gets parent of this point.
        /// </summary>
        IModule Parent { get; }

        /// <summary>
        ///     Event for data receiving handling.
        /// </summary>
        event EventHandler<object> DataReceived;
    }
}