using System;
using System.Collections.ObjectModel;
using System.IO;
using Fluid.Core.IO.Enums;
using Fluid.Core.IO.Interfaces;

namespace Fluid.Core.IO
{
    public class Computer : Directory
    {
        private DirectoryType _type = DirectoryType.Pc;

        private ObservableCollection<IFileSystemObject> _children = new ObservableCollection<IFileSystemObject>();

        /// <summary>
        ///     Новый экземпляр директории.
        /// </summary>
        /// <param name="parent">Родитель.</param>
        public Computer(IFileSystemObject parent)
        {
            try
            {
                Name = "Этот компьютер";
                FullName = "Этот компьютер";
                Parent = parent;
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
        public override DirectoryType Type
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
        public override ObservableCollection<IFileSystemObject> Children
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
        public override void LoadChildren()
        {
            Children.Clear();

            foreach (var drive in DriveInfo.GetDrives())
            {
                var directory = new Directory(drive.Name, this);

                directory.Name = directory.Name.Replace(@"\", string.Empty);
                directory.IsHidden = false;

                switch (drive.DriveType)
                {
                    case DriveType.Unknown:
                        directory.Type = DirectoryType.Unknown;
                        break;
                    case DriveType.NoRootDirectory:
                        directory.Type = DirectoryType.NoRootDirectory;
                        break;
                    case DriveType.Removable:
                        directory.Type = DirectoryType.Removable;
                        break;
                    case DriveType.Fixed:
                        directory.Type = DirectoryType.HardDrive;
                        break;
                    case DriveType.Network:
                        directory.Type = DirectoryType.NetworkDirectory;
                        break;
                    case DriveType.CDRom:
                        directory.Type = DirectoryType.OpticalDrive;
                        break;
                    case DriveType.Ram:
                        directory.Type = DirectoryType.Ram;
                        break;
                    default:
                        directory.Type = DirectoryType.Unknown;
                        break;
                }

                Children.Add(directory);
            }
        }
    }
}