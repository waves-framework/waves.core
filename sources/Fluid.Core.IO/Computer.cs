using System;
using System.Collections.ObjectModel;
using System.IO;
using Fluid.Core.IO.Enums;
using Fluid.Core.IO.Interfaces;

namespace Fluid.Core.IO
{
    /// <summary>
    ///     Computer directory.
    /// </summary>
    public class Computer : Directory
    {
        /// <summary>
        ///     Новый экземпляр директории.
        /// </summary>
        /// <param name="parent">Родитель.</param>
        public Computer(IFileSystemObject parent)
        {
            try
            {
                Name = "This PC";
                FullName = "Computer";
                Parent = parent;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        public override DirectoryType Type { get; internal set; } = DirectoryType.Computer;

        /// <inheritdoc />
        public override ObservableCollection<IFileSystemObject> Children { get; protected set; } =
            new ObservableCollection<IFileSystemObject>();

        /// <inheritdoc />
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