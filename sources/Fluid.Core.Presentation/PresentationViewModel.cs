using Fluid.Core.Base;
using Fluid.Core.Presentation.Interfaces;

namespace Fluid.Core.Presentation
{
    public abstract class PresentationViewModel : ObservableObject, IPresentationViewModel
    {
        /// <inheritdoc />
        public abstract void Initialize();
    }
}