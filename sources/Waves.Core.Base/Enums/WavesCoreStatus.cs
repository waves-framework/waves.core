namespace Waves.Core.Base.Enums
{
    /// <summary>
    /// Enum for Core statuses.
    /// </summary>
    public enum WavesCoreStatus
    {
        /// <summary>
        /// Core is not running.
        /// </summary>
        NotRunning,
        
        /// <summary>
        /// Core is starting right now.
        /// </summary>
        Starting,
        
        /// <summary>
        /// Core is running.
        /// </summary>
        Running,
        
        /// <summary>
        /// Core is stopped.
        /// </summary>
        Stopped,
        
        /// <summary>
        /// Core is stopping.
        /// </summary>
        Stopping,
        
        /// <summary>
        /// Core running failed.
        /// </summary>
        Failed
    }
}