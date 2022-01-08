using System.Drawing;

namespace Waves.Core.Plugins.Services.Log.Console
{
    /// <summary>
    ///     Colors for console.
    /// </summary>
    public static class Colors
    {
        /// <summary>
        ///     Date time color.
        /// </summary>
        public static readonly Color ConsoleDateTimeColor = Color.FromArgb(
            128,
            128,
            128);

        /// <summary>
        ///     Sender color.
        /// </summary>
        public static readonly Color ConsoleSenderColor = Color.FromArgb(
            224,
            224,
            224);

        /// <summary>
        ///     Sender color.
        /// </summary>
        public static readonly Color ConsoleMessageColor = Color.FromArgb(
            156,
            156,
            156);

        /// <summary>
        ///     Information color.
        /// </summary>
        public static readonly Color InformationColor = Color.LightGray;

        /// <summary>
        ///     Warning color.
        /// </summary>
        public static readonly Color WarningColor = Color.Yellow;

        /// <summary>
        ///     Success color.
        /// </summary>
        public static readonly Color SuccessColor = Color.SeaGreen;

        /// <summary>
        ///     Error color.
        /// </summary>
        public static readonly Color ErrorColor = Color.OrangeRed;

        /// <summary>
        ///     Fatal error color.
        /// </summary>
        public static readonly Color FatalErrorColor = Color.DarkRed;

        /// <summary>
        ///     Debug color.
        /// </summary>
        public static readonly Color DebugColor = Color.SandyBrown;

        /// <summary>
        ///     Verbose color.
        /// </summary>
        public static readonly Color VerboseColor = Color.SaddleBrown;

        /// <summary>
        ///     Verbose color.
        /// </summary>
        public static readonly Color TraceColor = Color.SlateGray;

        /// <summary>
        ///     Gets color for message labels.
        /// </summary>
        /// <param name="type">Message type.</param>
        /// <returns>Return color.</returns>
        public static Color GetColor(
            WavesMessageType type)
        {
            return type switch
            {
                WavesMessageType.Information => InformationColor,
                WavesMessageType.Warning => WarningColor,
                WavesMessageType.Error => ErrorColor,
                WavesMessageType.Fatal => FatalErrorColor,
                WavesMessageType.Success => SuccessColor,
                WavesMessageType.Debug => DebugColor,
                WavesMessageType.Verbose => VerboseColor,
                WavesMessageType.Trace => TraceColor,
                _ => new Color()
            };
        }
    }
}