using System.Windows.Input;

namespace Fluid.Core.Base.Interfaces
{
    public interface IAction : IObject
    {
        /// <summary>
        /// Описание действия.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Действие.
        /// </summary>
        ICommand Command { get; }
    }
}