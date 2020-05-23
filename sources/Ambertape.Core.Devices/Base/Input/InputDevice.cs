using Ambertape.Core.Devices.Interfaces.Input;

namespace Ambertape.Core.Devices.Base.Input
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