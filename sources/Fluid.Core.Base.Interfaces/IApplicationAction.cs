namespace Fluid.Core.Base.Interfaces
{
    public interface IApplicationAction : IAction
    {
        /// <summary>
        /// Иконка действия.
        /// </summary>
        string Icon { get; }
    }
}