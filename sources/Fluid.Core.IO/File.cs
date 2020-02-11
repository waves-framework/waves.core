using System;
using System.IO;
using Fluid.Core.Interfaces;

namespace Fluid.Core.IO
{
    public class File : FileSystemObject
    {
        private string _name;

        /// <summary>
        ///     Новый экземпляр файла.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="parent"></param>
        public File(string fullName, IFileSystemObject parent)
        {
            try
            {
                var fileInfo = new FileInfo(fullName);

                FullName = fileInfo.FullName;
                Name = fileInfo.Name;
                Parent = parent;
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
        public sealed override string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Расширение файла.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        ///     Размер файла.
        /// </summary>
        public long Size { get; set; }


    }
}