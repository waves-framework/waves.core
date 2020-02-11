using Fluid.Core.Enums;

namespace Fluid.Core.EventArgs
{
    public class KeyEventArgs
    {
        public bool Handled;

        /// <summary>
        ///     Создает новый экземпляр аргументов нажатия клавиши
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        /// <param name="ch"></param>
        public KeyEventArgs(KeyEventType eventType, VirtualKey key, KeyModifier modifier, char ch = default)
        {
            Modifier = modifier;
            Key = key;
            EventType = eventType;
            KeyCharacter = ch;
        }

        public KeyEventType EventType { get; }

        public VirtualKey Key { get; }

        public KeyModifier Modifier { get; }

        public char KeyCharacter { get; }

        public bool NoModifiersPressed => Modifier == KeyModifier.None;

        public bool IsPressedOnly(KeyModifier modifiers)
        {
            return modifiers == Modifier;
        }

        public bool IsPressed(KeyModifier modifiers)
        {
            return (modifiers & Modifier) > 0;
        }
    }
}