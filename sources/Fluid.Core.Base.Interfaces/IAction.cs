using System.Windows.Input;

namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for fluid's action.
    /// </summary>
    public interface IAction : IObject
    {
        /// <summary>
        /// Gets description.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets command.
        /// </summary>
        ICommand Command { get; }
    }
}