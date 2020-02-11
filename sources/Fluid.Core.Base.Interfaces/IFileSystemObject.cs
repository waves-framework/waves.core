namespace Fluid.Core.Base.Interfaces
{
    public interface IFileSystemObject : IObject
    {
        /// <summary>
        /// Выбран ли объект.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Является ли объект скрытым.
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        ///     Полный путь.
        /// </summary>
        string FullName { get; }

        /// <summary>
        ///     Родительский объект.
        /// </summary>
        IFileSystemObject Parent { get; }
    }
}