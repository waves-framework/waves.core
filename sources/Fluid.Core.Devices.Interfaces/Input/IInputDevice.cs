namespace Fluid.Core.Devices.Interfaces.Input
{
    public interface IInputDevice : IDevice
    {
        int NumberOfInputs { get; }
    }
}