using System.Collections.ObjectModel;
using Fluid.Core.Base;
using Fluid.Core.Presentation.Interfaces;

namespace Fluid.Core.Presentation
{
    public abstract class PresentationController : ObservableObject, IPresentationController
    {
        private IPresentation _selectedPresentation;

        private ObservableCollection<IPresentation> _presentations =
            new ObservableCollection<IPresentation>();

        /// <inheritdoc />
        public virtual IPresentation SelectedPresentation
        {
            get => _selectedPresentation;
            set
            {
                if (Equals(value, _selectedPresentation))
                {
                    return;
                }

                _selectedPresentation = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ObservableCollection<IPresentation> Presentations
        {
            get => _presentations;
            private set
            {
                if (Equals(value, _presentations))
                {
                    return;
                }

                _presentations = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public abstract void Initialize();
    }
}
