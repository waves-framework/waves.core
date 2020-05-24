using System.Windows.Input;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for Waves's action.
    /// </summary>
    public interface IAction : IObject
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