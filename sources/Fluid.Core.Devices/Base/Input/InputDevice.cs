using Fluid.Core.Devices.Interfaces.Input;

namespace Fluid.Core.Devices.Base.Input
{
    /// <summary>
    ///     Abstract input device base class.
    /// </summary>
    public abstract class InputDevice : Device, IInputDevice
    {
        /// <inheritdoc />
        public abstract int NumberOfInputs { get; }
    }
}