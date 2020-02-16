using System;
using Fluid.Core.Base.EventArgs;

namespace Fluid.Core.Services.Interfaces
{
    public interface IInputService : IService
    {
        /// <summary>
        ///     Событие нажатия клавиши
        /// </summary>
        event EventHandler<KeyEventArgs> KeyPressed;

        /// <summary>
        ///     Событие отпускания клавиши
        /// </summary>
        event EventHandler<KeyEventArgs> KeyReleased;

        /// <summary>
        ///     Событие мыши
        /// </summary>
        event EventHandler<PointerEventArgs> PointerStateChanged;

        /// <summary>
        ///     Установка нажатия клавиши
        /// </summary>
        /// <param name="e"></param>
        void SetKeyPressed(KeyEventArgs e);

        /// <summary>
        ///     Установка отпускания мыши
        /// </summary>
        /// <param name="e"></param>
        void SetKeyReleased(KeyEventArgs e);

        /// <summary>
        ///     Изменение состояния мыши
        /// </summary>
        /// <param name="e"></param>
        void SetPointer(PointerEventArgs e);
    }
}