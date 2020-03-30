using System.Collections.Generic;
using Fluid.Core.Base;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Devices.Interfaces.Input.ADC;

namespace Fluid.Core.Devices.Input.ADC
{
    public abstract class AdcEntryPoint : EntryPoint, IAdcEntryPoint
    {
        private double _managedGain;
        private double _digitalGain = 1.0f;

        private ICollection<double> _availableManagedGains;

        /// <inheritdoc />
        protected AdcEntryPoint(IModule parent, bool isProperty) : base(parent, isProperty)
        {
        }

        /// <inheritdoc />
        public double DigitalGain
        {
            get => _digitalGain;
            set
            {
                if (value.Equals(_digitalGain)) return;
                _digitalGain = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double ManagedGain
        {
            get => _managedGain;
            set
            {
                if (value.Equals(_managedGain)) return;
                _managedGain = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ICollection<double> AvailableManagedGains
        {
            get => _availableManagedGains;
            private set
            {
                if (Equals(value, _availableManagedGains)) return;
                _availableManagedGains = value;
                OnPropertyChanged();
            }
        }
    }
}