namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of application's action.
    /// </summary>
    public interface IWavesApplicationAction : IWavesAction
    {
        /// <summary>
        ///     Gets icon.
        /// </summary>
        string Icon { get; }
    }
}