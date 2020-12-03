using System.Collections.Generic;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Devices.Interfaces.Input.ADC;

namespace Waves.Core.Devices.Base.Input.ADC
{
    /// <summary>
    ///     Abstract ADC entry point base class.
    /// </summary>
    public abstract class AdcEntryPoint : WavesEntryPoint, IAdcEntryPoint
    {
        /// <inheritdoc />
        protected AdcEntryPoint(IWavesModule parent, bool isProperty) : base(parent, isProperty)
        {
        }

        /// <inheritdoc />
        public virtual double DigitalGain { get; set; } = 1.0f;

        /// <inheritdoc />
        public abstract double DeviceGain { get; set; }

        /// <inheritdoc />
        public ICollection<double> AvailableDeviceGains { get; private set; }
    }
}