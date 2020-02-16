using System;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.EventArgs;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    public class InputService : Service, IInputService
    {
        /// <summary>
        ///     Событие нажатия клавиши
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyPressed;

        /// <summary>
        ///     Событие отпускания клавиши
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyReleased;

        /// <summary>
        ///     Событие мыши
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerStateChanged;

        /// <summary>
        ///     Установка нажатия клавиши
        /// </summary>
        /// <param name="e"></param>
        public void SetKeyPressed(KeyEventArgs e)
        {
            OnKeyPressed(e);
        }

        /// <summary>
        ///     Установка отпускания мыши
        /// </summary>
        /// <param name="e"></param>
        public void SetKeyReleased(KeyEventArgs e)
        {
            OnKeyReleased(e);
        }

        /// <summary>
        ///     Изменение состояния мыши
        /// </summary>
        /// <param name="e"></param>
        public void SetPointer(PointerEventArgs e)
        {
            OnPointerStateChanged(e);
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("3F339B93-AE63-4F93-9DCD-F71FA378744E");

        /// <inheritdoc />
        public override string Name { get; set; } = "Keyboard and Mouse Input Service";

        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized) return;

            OnMessageReceived(this,
                new Message("Информация", "Сервис инициализирован.", Name, MessageType.Information));

            IsInitialized = true;
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }

        /// <summary>
        ///     Уведомление о нажатии клавиши
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        /// <summary>
        ///     Уведомление об отпускании клавиши
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnKeyReleased(KeyEventArgs e)
        {
            KeyReleased?.Invoke(this, e);
        }

        /// <summary>
        ///     Уведомление о событиях мыши
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPointerStateChanged(PointerEventArgs e)
        {
            PointerStateChanged?.Invoke(this, e);
        }
    }
}