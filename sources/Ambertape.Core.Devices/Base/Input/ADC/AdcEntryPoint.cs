using System.Collections.Generic;
using Ambertape.Core.Base;
using Ambertape.Core.Base.Interfaces;
using Ambertape.Core.Devices.Interfaces.Input.ADC;

namespace Ambertape.Core.Devices.Base.Input.ADC
{
    /// <summary>
    ///     Abstract ADC entry point base class.
    /// </summary>
    public abstract class AdcEntryPoint : EntryPoint, IAdcEntryPoint
    {
        /// <inheritdoc />
        protected AdcEntryPoint(IModule parent, bool isProperty) : base(parent, isProperty)
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