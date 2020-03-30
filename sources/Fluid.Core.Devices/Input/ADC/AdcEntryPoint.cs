using System.Collections.Generic;
using Fluid.Core.Base;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Devices.Interfaces.Input.ADC;

namespace Fluid.Core.Devices.Input.ADC
{
    /// <summary>
    /// Abstract ADC entry point base class.
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