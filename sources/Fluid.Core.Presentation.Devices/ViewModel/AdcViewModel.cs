using Fluid.Core.Devices.Interfaces.Input.ADC;

namespace Fluid.Core.Presentation.Devices.ViewModel
{
    public class AdcViewModel : PresentationViewModel
    {
        private IAdc _adc;
        private bool _isUsing;

        /// <summary>
        /// Новый экземпляр представления модели модуля АЦП.
        /// </summary>
        /// <param name="device"></param>
        public AdcViewModel(IAdc device)
        {
            Adc = device;
        }

        /// <summary>
        /// Используется ли данное АЦП.
        /// </summary>
        public bool IsUsing
        {
            get => _isUsing;
            set
            {
                if (value == _isUsing) return;
                _isUsing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// АЦП.
        /// </summary>
        public IAdc Adc
        {
            get => _adc;
            private set
            {
                if (Equals(value, _adc)) return;
                _adc = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override void Initialize()
        {
        }
    }
}