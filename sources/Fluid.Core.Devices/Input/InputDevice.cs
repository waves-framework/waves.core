using Fluid.Core.Devices.Interfaces.Input;

namespace Fluid.Core.Devices.Input
{
    public abstract class InputDevice : Device, IInputDevice
    {
        /// <inheritdoc />
        public abstract int NumberOfInputs { get; }
    }
}