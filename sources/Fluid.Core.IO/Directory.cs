using System;
using System.Collections.ObjectModel;
using System.IO;
using Fluid.Core.IO.Enums;
using Fluid.Core.IO.Interfaces;

namespace Fluid.Core.IO
{
    /// <summary>
    ///     Directory.
    /// </summary>
    public class Directory : FileSystemObject
    {
        private readonly DirectoryInfo _directoryInfo;

        /// <summary>
        ///     Creates new instance of Directory.
        /// </summary>
        public Directory()
        {
        }

        /// <summary>
        ///     Creates new instance of Directory.
        /// </summary>
        /// <param name="fullName">Full name</param>
        public Directory(string fullName)
        {
            try
            {
                _directoryInfo = new DirectoryInfo(fullName);

                FullName = _directoryInfo.FullName;
                Name = _directoryInfo.Name;
                Parent = _directoryInfo.Parent == null ? null : new Directory(_directoryInfo.Parent.FullName);

                if (_directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    IsHidden = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///     Creates new instance of Directory.
        /// </summary>
        /// <param name="fullName">Full name.</param>
        /// <param name="parent">Parent.</param>
        public Directory(string fullName, IFileSystemObject parent)
        {
            try
            {
                _directoryInfo = new DirectoryInfo(fullName);

                FullName = _directoryInfo.FullName;
                Name = _directoryInfo.Name;
                Parent = parent;

                if (_directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
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
        ///     Directory type.
        /// </summary>
        public virtual DirectoryType Type { get; internal set; } = DirectoryType.Directory;

        /// <summary>
        ///     Directory children collection.
        /// </summary>
        public virtual ObservableCollection<IFileSystemObject> Children { get; protected set; } =
            new ObservableCollection<IFileSystemObject>();

        /// <summary>
        ///     Loads directory's children.
        /// </summary>
        public virtual void LoadChildren()
        {
            Children.Clear();

            foreach (var directory in _directoryInfo.GetDirectories())
                if (!directory.Attributes.HasFlag(FileAttributes.Hidden))
                    Children.Add(new Directory(directory.FullName, this));

            foreach (var file in _directoryInfo.GetFiles())
                Children.Add(new File(file.FullName, this));
        }
    }
}