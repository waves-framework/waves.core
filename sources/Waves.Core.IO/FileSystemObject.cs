using Waves.Core.Base;
using Waves.Core.IO.Interfaces;

namespace Waves.Core.IO
{
    /// <summary>
    ///     Abstract base file system object class.
    /// </summary>
    public abstract class FileSystemObject : WavesObject, IFileSystemObject
    {
        /// <summary>
        ///     Creates new instance of file system object.
        /// </summary>
        protected FileSystemObject()
        {
        }

        /// <summary>
        ///     Creates new instance of file system object.
        /// </summary>
        /// <param name="fullName">Full Name.</param>
        /// <param name="parent">Parent.</param>
        protected FileSystemObject(string fullName, IFileSystemObject parent)
        {
            FullName = fullName;
            Parent = parent;
        }

        /// <inheritdoc />
        public bool IsHidden { get; internal set; }

        /// <inheritdoc />
        public override string Name { get; set; }

        /// <inheritdoc />
        public string FullName { get; protected set; }

        /// <inheritdoc />
        public IFileSystemObject Parent { get; protected set; }
    }
}