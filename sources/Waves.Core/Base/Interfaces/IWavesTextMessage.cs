namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    /// Text message interface.
    /// </summary>
    public interface IWavesTextMessage : IWavesMessageObject
    {
        /// <summary>
        /// Gets message text.
        /// </summary>
        string Text { get; }
    }
}
