namespace Fluid.Core.Devices.Interfaces.Output
{
    public interface IOutputDevice : IDevice
    {
        int NumberOfOutputs { get; }
    }
}