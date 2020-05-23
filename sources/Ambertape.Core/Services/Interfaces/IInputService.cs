using System;
using Ambertape.Core.Base.EventArgs;
using Ambertape.Core.Base.Interfaces;

namespace Ambertape.Core.Services.Interfaces
{
    /// <summary>
    ///     Interface for input service classes.
    /// </summary>
    public interface IInputService : IService
    {
        /// <summary>
        ///     Event for key pressing.
        /// </summary>
        event EventHandler<KeyEventArgs> KeyPressed;

        /// <summary>
        ///     Event for key releasing.
        /// </summary>
        event EventHandler<KeyEventArgs> KeyReleased;

        /// <summary>
        ///     Event for pointer state changing.
        /// </summary>
        event EventHandler<PointerEventArgs> PointerStateChanged;

        /// <summary>
        ///     Sets key pressed state.
        /// </summary>
        /// <param name="e">Arguments.</param>
        void SetKeyPressed(KeyEventArgs e);

        /// <summary>
        ///     Sets key released state.
        /// </summary>
        /// <param name="e">Arguments.</param>
        void SetKeyReleased(KeyEventArgs e);

        /// <summary>
        ///     Sets pointer state.
        /// </summary>
        /// <param name="e">Arguments.</param>
        void SetPointer(PointerEventArgs e);
    }
}