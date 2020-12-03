using System;
using Waves.Core.Base.EventArgs;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    ///     Interface for input service classes.
    /// </summary>
    public interface IInputService : IWavesService
    {
        /// <summary>
        ///     Event for key pressing.
        /// </summary>
        event EventHandler<WavesKeyEventArgs> KeyPressed;

        /// <summary>
        ///     Event for key releasing.
        /// </summary>
        event EventHandler<WavesKeyEventArgs> KeyReleased;

        /// <summary>
        ///     Event for pointer state changing.
        /// </summary>
        event EventHandler<WavesPointerEventArgs> PointerStateChanged;

        /// <summary>
        ///     Sets key pressed state.
        /// </summary>
        /// <param name="e">Arguments.</param>
        void SetKeyPressed(WavesKeyEventArgs e);

        /// <summary>
        ///     Sets key released state.
        /// </summary>
        /// <param name="e">Arguments.</param>
        void SetKeyReleased(WavesKeyEventArgs e);

        /// <summary>
        ///     Sets pointer state.
        /// </summary>
        /// <param name="e">Arguments.</param>
        void SetPointer(WavesPointerEventArgs e);
    }
}