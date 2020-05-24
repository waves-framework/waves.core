using System.Collections.Generic;

namespace Waves.Core.Devices.Interfaces.Input.ADC
{
    /// <summary>
    ///     Interface for ADC device instances.
    /// </summary>
    public interface IAdc : IInputDevice
    {
        /// <summary>
        ///     Gets sample rate.
        /// </summary>
        double SampleRate { get; set; }

        /// <summary>
        ///     Gets bits per sample.
        /// </summary>
        short BitsPerSample { get; set; }

        /// <summary>
        ///     Gets available sample rates collection.
        /// </summary>
        ICollection<double> AvailableSampleRates { get; }

        /// <summary>
        ///     Gets available bits per sample collection.
        /// </summary>
        ICollection<short> AvailableBitsPerSampleValues { get; }
    }
}