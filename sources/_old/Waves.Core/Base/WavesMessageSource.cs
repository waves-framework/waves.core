namespace Waves.Core.Base
{
    /// <summary>
    /// Message source.
    /// </summary>
    public class WavesMessageSource : WavesObject
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesMessageSource"/>.
        /// </summary>
        /// <param name="source">Source.</param>
        public WavesMessageSource(string source)
        {
            Source = source;
        }

        /// <summary>
        /// Source.
        /// </summary>
        public string Source { get; set; }
    }
}
