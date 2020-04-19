using System;
using System.Composition;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.EventArgs;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services
{
    /// <summary>
    ///     Input service.
    /// </summary>
    [Export(typeof(IService))]
    public class InputService : Service, IInputService
    {
        /// <inheritdoc />
        public event EventHandler<KeyEventArgs> KeyPressed;

        /// <inheritdoc />
        public event EventHandler<KeyEventArgs> KeyReleased;

        /// <inheritdoc />
        public event EventHandler<PointerEventArgs> PointerStateChanged;

        /// <inheritdoc />
        public void SetKeyPressed(KeyEventArgs e)
        {
            OnKeyPressed(e);
        }

        /// <inheritdoc />
        public void SetKeyReleased(KeyEventArgs e)
        {
            OnKeyReleased(e);
        }

        /// <inheritdoc />
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
                new Message("Initialization", "Service was initialized.", Name, MessageType.Information));

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
        ///     Notifies when key pressed.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        /// <summary>
        ///     Notifies when key released.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        protected virtual void OnKeyReleased(KeyEventArgs e)
        {
            KeyReleased?.Invoke(this, e);
        }

        /// <summary>
        ///     Notifies when pointer state changed.
        /// </summary>
        /// <param name="e">Pointer event arguments.</param>
        protected virtual void OnPointerStateChanged(PointerEventArgs e)
        {
            PointerStateChanged?.Invoke(this, e);
        }
    }
}