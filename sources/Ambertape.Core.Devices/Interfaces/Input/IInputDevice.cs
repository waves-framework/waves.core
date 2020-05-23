namespace Ambertape.Core.Devices.Interfaces.Input
{
    /// <summary>
    ///     Interface for input devices instances.
    /// </summary>
    public interface IInputDevice : IDevice
    {
        /// <summary>
        ///     Gets number of device inputs.
        /// </summary>
        int NumberOfInputs { get; }
    }
}