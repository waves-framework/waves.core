using System;
using System.Collections.ObjectModel;
using System.IO;
using Fluid.Core.IO.Enums;
using Fluid.Core.IO.Interfaces;

namespace Fluid.Core.IO
{
    public class Directory : FileSystemObject
    {
        private string _name;

        private readonly DirectoryInfo _directoryInfo;

        private DirectoryType _type = DirectoryType.Directory;

        private ObservableCollection<IFileSystemObject> _children = new ObservableCollection<IFileSystemObject>();

        /// <summary>
        ///     Новый экземпляр директории.
        /// </summary>
        public Directory()
        {
        }

        /// <summary>
        ///     Новый экземпляр директории.
        /// </summary>
        /// <param name="fullName"></param>
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
        ///     Новый экземпляр директории.
        /// </summary>
        /// <param name="fullName">Полное имя.</param>
        /// <param name="parent">Родитель.</param>
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

        /// <summary>
        ///     Тип директории
        /// </summary>
        public virtual DirectoryType Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Объекты, содержащиеся в директории
        /// </summary>
        public virtual ObservableCollection<IFileSystemObject> Children
        {
            get => _children;
            set
            {
                if (Equals(value, _children)) return;
                _children = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Загрузка объектов, содержащихся в папке
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