using System;
using System.IO;
using Fluid.Core.IO.Interfaces;

namespace Fluid.Core.IO
{
    /// <summary>
    /// File.
    /// </summary>
    public class File : FileSystemObject
    {
        /// <summary>
        ///     Creates new instance of File.
        /// </summary>
        /// <param name="fullName">Full Name.</param>
        /// <param name="parent">Parent.</param>
        public File(string fullName, IFileSystemObject parent) : base(fullName, parent)
        {
            try
            {
                var fileInfo = new FileInfo(fullName);

                Name = fileInfo.Name;
                Extension = fileInfo.Extension;
                Size = fileInfo.Length;
                Id = Guid.NewGuid();

                if (fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    IsHidden = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public sealed override string Name { get; set; }

        /// <summary>
        ///     File's extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        ///     File's size.
        /// </summary>
        public long Size { get; set; }
    }
}