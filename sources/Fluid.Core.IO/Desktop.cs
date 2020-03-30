using System;
using System.Collections.ObjectModel;
using System.IO;
using Fluid.Core.IO.Enums;
using Fluid.Core.IO.Interfaces;

namespace Fluid.Core.IO
{
    /// <summary>
    /// Desktop.
    /// </summary>
    public class Desktop : Directory
    {
        private readonly Directory _computer;
        private readonly Directory _userDirectory;
        private readonly DirectoryInfo _directoryInfo;

        /// <summary>
        ///     Creates new instance of desktop directory.
        /// </summary>
        public Desktop()
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                _directoryInfo = new DirectoryInfo(desktopPath);

                FullName = _directoryInfo.FullName;
                Name = "Desktop";      // TODO: Localization. 
                Parent = null;

                _computer = new Computer(this);
                _userDirectory = new Directory(userPath, this) {Type = DirectoryType.User};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        public override DirectoryType Type { get; internal set; } = DirectoryType.Desktop;

        /// <inheritdoc />
        public override ObservableCollection<IFileSystemObject> Children { get; protected set; } = new ObservableCollection<IFileSystemObject>();

        /// <inheritdoc />
        public override void LoadChildren()
        {
            Children.Clear();

            Children.Add(_computer);
            Children.Add(_userDirectory);

            foreach (var directory in _directoryInfo.GetDirectories())
                if (!directory.Attributes.HasFlag(FileAttributes.Hidden))
                    Children.Add(new Directory(directory.FullName, this));

            foreach (var file in _directoryInfo.GetFiles())
                Children.Add(new File(file.FullName, this));
        }
    }
}