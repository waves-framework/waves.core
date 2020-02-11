using Fluid.Core.Devices.Interfaces.Output;

namespace Fluid.Core.Devices.Output
{
    public abstract class OutputDevice : Device, IOutputDevice
    {
        /// <inheritdoc />
        public abstract int NumberOfOutputs { get; }
    }
}