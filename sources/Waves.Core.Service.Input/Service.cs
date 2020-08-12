using System;
using System.Composition;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.EventArgs;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Services.Input
{
    /// <summary>
    ///     Input service.
    /// </summary>
    [Export(typeof(IService))]
    public class Service : Base.Service, IInputService
    {
        /// <inheritdoc />
        public event EventHandler<KeyEventArgs> KeyPressed;

        /// <inheritdoc />
        public event EventHandler<KeyEventArgs> KeyReleased;

        /// <inheritdoc />
        public event EventHandler<PointerEventArgs> PointerStateChanged;

        /// <summary>
        ///     Gets instance of Core.
        /// </summary>
        public ICore Core { get; private set; }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("3F339B93-AE63-4F93-9DCD-F71FA378744E");

        /// <inheritdoc />
        public override string Name { get; set; } = "Keyboard and Mouse Input Service";

        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            if (IsInitialized) return;

            Core = core;

            OnMessageReceived(this,
                new Message("Initialization", "Service has been initialized.", Name, MessageType.Information));

            IsInitialized = true;
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            OnMessageReceived(this, new Message("Loading configuration", "There is nothing to load.",
                Name,
                MessageType.Success));
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            OnMessageReceived(this, new Message("Saving configuration", "There is nothing to save.",
                Name,
                MessageType.Success));
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }

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