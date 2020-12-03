using System;
using System.Composition;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.EventArgs;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Service.Input
{
    /// <summary>
    ///     Input service.
    /// </summary>
    [Export(typeof(IWavesService))]
    public class Service : WavesService, IInputService
    {
        /// <inheritdoc />
        public event EventHandler<WavesKeyEventArgs> KeyPressed;

        /// <inheritdoc />
        public event EventHandler<WavesKeyEventArgs> KeyReleased;

        /// <inheritdoc />
        public event EventHandler<WavesPointerEventArgs> PointerStateChanged;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("3F339B93-AE63-4F93-9DCD-F71FA378744E");

        /// <inheritdoc />
        public override string Name { get; set; } = "Keyboard and Mouse Input Service";

        /// <inheritdoc />
        public override void Initialize(IWavesCore core)
        {
            if (IsInitialized) return;

            Core = core;

            OnMessageReceived(
                this,
                new WavesMessage(
                    "Initialization", 
                    "Service has been initialized.", 
                    Name, 
                    WavesMessageType.Information));

            IsInitialized = true;
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            OnMessageReceived(this, 
                new WavesMessage(
                    "Loading configuration", 
                    "There is nothing to load.",
                Name,
                WavesMessageType.Information));
        }

        /// <inheritdoc />
        public override void SaveConfiguration()
        {
            OnMessageReceived(
                this, 
                new WavesMessage(
                "Saving configuration", 
                "There is nothing to save.",
                Name,
                WavesMessageType.Information));
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }

        /// <inheritdoc />
        public void SetKeyPressed(WavesKeyEventArgs e)
        {
            OnKeyPressed(e);
        }

        /// <inheritdoc />
        public void SetKeyReleased(WavesKeyEventArgs e)
        {
            OnKeyReleased(e);
        }

        /// <inheritdoc />
        public void SetPointer(WavesPointerEventArgs e)
        {
            OnPointerStateChanged(e);
        }

        /// <summary>
        ///     Notifies when key pressed.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        protected virtual void OnKeyPressed(WavesKeyEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        /// <summary>
        ///     Notifies when key released.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        protected virtual void OnKeyReleased(WavesKeyEventArgs e)
        {
            KeyReleased?.Invoke(this, e);
        }

        /// <summary>
        ///     Notifies when pointer state changed.
        /// </summary>
        /// <param name="e">Pointer event arguments.</param>
        protected virtual void OnPointerStateChanged(WavesPointerEventArgs e)
        {
            PointerStateChanged?.Invoke(this, e);
        }
    }
}