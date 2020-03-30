using System;

namespace Fluid.Core.Base.Enums
{
    /// <summary>
    /// Key modifiers enum.
    /// </summary>
    [Flags]
    public enum KeyModifier
    {
        /// <summary>
        /// None key.
        /// </summary>
        None = 0b0000,

        /// <summary>
        /// Alt key.
        /// </summary>
        Alt = 0b0001,

        /// <summary>
        /// Ctrl key.
        /// </summary>
        Ctrl = 0b0010,

        /// <summary>
        /// Shift key.
        /// </summary>
        Shift = 0b0100,

        /// <summary>
        /// Win key.
        /// </summary>
        Win = 0b1000
    }
}