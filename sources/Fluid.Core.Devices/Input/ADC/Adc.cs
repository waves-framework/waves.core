using System.Collections.Generic;
using Fluid.Core.Devices.Interfaces.Input.ADC;

namespace Fluid.Core.Devices.Input.ADC
{
    public abstract class Adc : InputDevice, IAdc
    {
        private double _sampleRate;
        private short _bitsPerSample;

        private ICollection<double> _availableSampleRates = new List<double>();
        private ICollection<short> _availableBitsPerSampleValues = new List<short>();

        /// <inheritdoc />
        public double SampleRate
        {
            get => _sampleRate;
            set
            {
                if (value.Equals(_sampleRate)) return;
                _sampleRate = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public short BitsPerSample
        {
            get => _bitsPerSample;
            set
            {
                if (value == _bitsPerSample) return;
                _bitsPerSample = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ICollection<double> AvailableSampleRates
        {
            get => _availableSampleRates;
            private set
            {
                if (Equals(value, _availableSampleRates)) return;
                _availableSampleRates = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ICollection<short> AvailableBitsPerSampleValues
        {
            get => _availableBitsPerSampleValues;
            private set
            {
                if (Equals(value, _availableBitsPerSampleValues)) return;
                _availableBitsPerSampleValues = value;
                OnPropertyChanged();
            }
        }
    }
}