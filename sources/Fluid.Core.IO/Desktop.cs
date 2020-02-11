using System;
using System.Collections.ObjectModel;
using System.IO;
using Fluid.Core.Interfaces;
using Fluid.Core.IO.Enums;

namespace Fluid.Core.IO
{
    public class Desktop : Directory
    {
        private readonly Directory _computer;
        private readonly Directory _userDirectory;

        private readonly DirectoryInfo _directoryInfo;

        private DirectoryType _type = DirectoryType.Desktop;

        private ObservableCollection<IFileSystemObject> _children = new ObservableCollection<IFileSystemObject>();

        /// <summary>
        ///     Новый экземпляр директории.
        /// </summary>
        public Desktop()
        {
            try
            {
                var desktopPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var userPath = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                _directoryInfo = new DirectoryInfo(desktopPath);

                FullName = _directoryInfo.FullName;
                Name = "Рабочий стол";
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