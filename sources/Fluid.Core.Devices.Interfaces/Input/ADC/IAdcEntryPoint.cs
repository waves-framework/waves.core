using System.Collections.Generic;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Devices.Interfaces.Input.ADC
{
    public interface IAdcEntryPoint : IEntryPoint
    {
        /// <summary>
        ///     Цифровое усиление.
        /// </summary>
        double DigitalGain { get; set; }

        /// <summary>
        ///     Цифровое усиление.
        /// </summary>
        double ManagedGain { get; set; }

        /// <summary>
        ///     Список усилений.
        /// </summary>
        ICollection<double> AvailableManagedGains { get; }
    }
}