using Fluid.Core.Base.Enums;

namespace Fluid.Core.Base.EventArgs
{
    /// <summary>
    ///     Event args for keyboard events.
    /// </summary>
    public class KeyEventArgs
    {
        /// <summary>
        ///     Creates new instance of key event args.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        /// <param name="key">Virtual key.</param>
        /// <param name="modifier">Key modifier.</param>
        /// <param name="ch">Key character.</param>
        public KeyEventArgs(KeyEventType eventType, VirtualKey key, KeyModifier modifier, char ch = default)
        {
            Modifier = modifier;
            Key = key;
            EventType = eventType;
            KeyCharacter = ch;
        }

        /// <summary>
        ///     Gets key event type.
        /// </summary>
        public KeyEventType EventType { get; }

        /// <summary>
        ///     Gets virtual key.
        /// </summary>
        public VirtualKey Key { get; }

        /// <summary>
        ///     Gets modifier.
        /// </summary>
        public KeyModifier Modifier { get; }

        /// <summary>
        ///     Gets key character.
        /// </summary>
        public char KeyCharacter { get; }

        /// <summary>
        ///     Gets whether the modifiers pressed.
        /// </summary>
        public bool NoModifiersPressed => Modifier == KeyModifier.None;

        /// <summary>
        ///     Gets whether key is pressed only.
        /// </summary>
        /// <param name="modifiers">Modifier.</param>
        /// <returns>True if pressed, false if not.</returns>
        public bool IsPressedOnly(KeyModifier modifiers)
        {
            return modifiers == Modifier;
        }

        /// <summary>
        ///     Gets whether key is pressed.
        /// </summary>
        /// <param name="modifiers">Modifier.</param>
        /// <returns>True if pressed, false if not.</returns>
        public bool IsPressed(KeyModifier modifiers)
        {
            return (modifiers & Modifier) > 0;
        }
    }
}