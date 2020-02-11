using System.Windows;
using Fluid.Core.Base;
using Fluid.Core.Presentation.Interfaces;

namespace Fluid.Core.Presentation.Base
{
    public abstract class Presentation : ObservableObject, IPresentation
    {
        /// <inheritdoc />
        public abstract object DataContext { get; }

        /// <inheritdoc />
        public abstract FrameworkElement View { get; }

        /// <inheritdoc />
        public abstract void Initialize();
    }
}
