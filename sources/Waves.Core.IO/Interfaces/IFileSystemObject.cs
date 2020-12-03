using Waves.Core.Base.Interfaces;

namespace Waves.Core.IO.Interfaces
{
    /// <summary>
    ///     Interface for file system object.
    /// </summary>
    public interface IFileSystemObject : IWavesObject
    {
        /// <summary>
        ///     Gets whether object is hidden.
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        ///     Gets full name of object (full path).
        /// </summary>
        string FullName { get; }

        /// <summary>
        ///     Gets object's parent.
        /// </summary>
        IFileSystemObject Parent { get; }
    }
}