using System.Windows.Input;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for Waves action.
    /// </summary>
    public interface IWavesAction : IWavesObject
    {
        /// <summary>
        ///     Gets description.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets command.
        /// </summary>
        ICommand Command { get; }
    }
}