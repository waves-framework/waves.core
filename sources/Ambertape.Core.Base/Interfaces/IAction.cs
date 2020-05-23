using System.Windows.Input;

namespace Ambertape.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for Ambertape's action.
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