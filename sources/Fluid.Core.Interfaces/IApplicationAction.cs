using System;
using System.Windows.Input;

namespace Fluid.Core.Interfaces
{
    public interface IApplicationAction : IAction
    {
        /// <summary>
        /// Иконка действия.
        /// </summary>
        string Icon { get; }
    }
}