using System.Collections.Generic;

namespace Fluid.Core.Devices.Interfaces.Input.ADC
{
    public interface IAdc : IInputDevice
    {
        /// <summary>
        ///     Частота дискретизации.
        /// </summary>
        double SampleRate { get; set; }

        /// <summary>
        ///     Разрядность.
        /// </summary>
        short BitsPerSample { get; set; }

        /// <summary>
        ///     Доступные частоты дискретизации.
        /// </summary>
        ICollection<double> AvailableSampleRates { get; }

        /// <summary>
        ///     Доступные значения разрядности.
        /// </summary>
        ICollection<short> AvailableBitsPerSampleValues { get; }
    }
}