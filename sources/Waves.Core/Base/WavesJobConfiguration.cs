using Waves.Core.Base.Enums;

namespace Waves.Core.Base
{
    /// <summary>
    /// Job configuration model.
    /// </summary>
    public class WavesJobConfiguration
    {
        /// <summary>
        /// Gets or sets job name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether job execution needed to repeat forever.
        /// </summary>
        public bool RepeatForever { get; set; }

        /// <summary>
        /// Gets or sets job execute interval.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets job execute interval unit.
        /// </summary>
        public WavesJobIntervalUnit IntervalUnit { get; set; }

        /// <summary>
        /// Gets or sets job execute offset.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets job execute offset unit.
        /// </summary>
        public WavesJobIntervalUnit OffsetUnit { get; set; }
    }
}
