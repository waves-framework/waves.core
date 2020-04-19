using System.Collections.Generic;
using Fluid.Core.Devices.Interfaces.Input.ADC;

namespace Fluid.Core.Devices.Base.Input.ADC
{
    /// <summary>
    ///     Abstract ADC device base class.
    /// </summary>
    public abstract class Adc : InputDevice, IAdc
    {
        /// <inheritdoc />
        public virtual double SampleRate { get; set; }

        /// <inheritdoc />
        public virtual short BitsPerSample { get; set; }

        /// <inheritdoc />
        public ICollection<double> AvailableSampleRates { get; } = new List<double>();

        /// <inheritdoc />
        public ICollection<short> AvailableBitsPerSampleValues { get; } = new List<short>();
    }
}