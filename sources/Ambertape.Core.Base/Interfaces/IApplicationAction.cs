namespace Ambertape.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of application's action.
    /// </summary>
    public interface IApplicationAction : IAction
    {
        /// <summary>
        ///     Gets icon.
        /// </summary>
        string Icon { get; }
    }
}