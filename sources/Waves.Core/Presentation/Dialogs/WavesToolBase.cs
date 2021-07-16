using ReactiveUI;
using Waves.Core.Presentation.Dialogs.Interfaces;

namespace Waves.Core.Presentation.Dialogs
{
    /// <summary>
    /// Dialog action base.
    /// </summary>
    public abstract class WavesToolBase : ReactiveObject, IWavesTool
    {
        /// <inheritdoc />
        public abstract string Caption { get; }

        /// <inheritdoc />
        public abstract string ToolTip { get; }

        /// <inheritdoc />
        public abstract void Initialize();
    }
}
